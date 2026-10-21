
// using CleanArchitecture.Blazor.Application.Features.SmsMessages.DTOs;
// using CleanArchitecture.Blazor.Domain.Enums;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Logging;


// namespace CleanArchitecture.Blazor.Application.Features.SmsMessages.Commands.PersistSms;

// public record PersistInboundSmsCommand(InboundSmsDto Sms) : IRequest<Result<bool>>;

// public class PersistInboundSmsCommandHandler
//     : IRequestHandler<PersistInboundSmsCommand, Result<bool>>
// {
//     private readonly IApplicationDbContextFactory _dbContextFactory;
//     private readonly IApplicationHubWrapper _hub;
//     private readonly ILogger<PersistInboundSmsCommandHandler> _log;

//     public PersistInboundSmsCommandHandler(
//         IApplicationDbContextFactory dbContextFactory,
//         IApplicationHubWrapper hub,
//         ILogger<PersistInboundSmsCommandHandler> log)
//     {
//         _dbContextFactory          = dbContextFactory;
//         _hub         = hub;
//         _log         = log;
//     }

//     public async ValueTask<Result<bool>> Handle(PersistInboundSmsCommand req, CancellationToken ct)
//     {

//         await using var _db = await _dbContextFactory.CreateAsync(ct);
//         var s = req.Sms;

//         // Idempotency: skip if this gateway id is already stored.

//         var already = await _db.SmsMessages
//             .AnyAsync(x => x.Direction == SmsDirection.Inbound
//                         && x.GatewayId == s.GatewayId.ToString(), ct);
//         if (already)
//         {
//             _log.LogDebug("Inbound SMS gatewayId={GatewayId} already stored; skipping.", s.GatewayId);
//             return await Result<bool>.SuccessAsync(false);
//         }

//         var receivedAt = DateTimeOffset
//             .FromUnixTimeMilliseconds(s.Ts)
//             .UtcDateTime;

//         var entity = new SmsMessage
//         {
//             Direction  = SmsDirection.Inbound,
//             From       = s.From,
//             To         = "self",
//             Body       = s.Body,
//             GatewayId  = s.GatewayId.ToString(),
//             SimSlot    = s.SimSlot ?? 0,
//             ReceivedAt = receivedAt,
//             Status     = SmsStatus.Delivered
//         };

//         try
//         {
//             entity.AddDomainEvent(new SmsMessageCreatedEvent(entity));
//             _db.SmsMessages.Add(entity);
//             await _db.SaveChangesAsync(ct);
//         }
//         catch (DbUpdateException)
//         {
//             // Concurrent insert — unique index on GatewayId caught the race.
//             _log.LogDebug(
//                 "Concurrent inbound insert for gatewayId={GatewayId}; treating as duplicate.",
//                 s.GatewayId);
//             return await Result<bool>.SuccessAsync(false);
//         }

//         _log.LogInformation("Inbound SMS stored: gatewayId={GatewayId} from={From}",
//                             s.GatewayId, s.From);

//         await _hub.SmsReceived(new SmsReceivedPayload(
//             Id:         entity.Id,
//             GatewayId:  s.GatewayId,
//             From:       s.From,
//             Body:       s.Body,
//             Ts:         s.Ts,
//             ReceivedAt: receivedAt
//             ));

//         return await Result<bool>.SuccessAsync(true);
//     }
// }


using CleanArchitecture.Blazor.Application.Features.SmsMessages.DTOs;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Blazor.Application.Features.SmsMessages.Commands.PersistSms;

public record PersistInboundSmsCommand(InboundSmsDto Sms) : IRequest<Result<bool>>;

public class PersistInboundSmsCommandHandler
    : IRequestHandler<PersistInboundSmsCommand, Result<bool>>
{
    // How long we accept late parts before declaring a group abandoned.
    private static readonly TimeSpan AssemblyWindow = TimeSpan.FromMinutes(5);

    // Small grace period to let a concurrent sibling transaction commit
    // before we decide "waiting for more parts".
    private static readonly TimeSpan RaceGrace = TimeSpan.FromMilliseconds(150);

    private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IApplicationHubWrapper         _hub;
    private readonly ILogger<PersistInboundSmsCommandHandler> _log;

    public PersistInboundSmsCommandHandler(
        IApplicationDbContextFactory dbContextFactory,
        IApplicationHubWrapper hub,
        ILogger<PersistInboundSmsCommandHandler> log)
    {
        _dbContextFactory = dbContextFactory;
        _hub              = hub;
        _log              = log;
    }

    public async ValueTask<Result<bool>> Handle(
        PersistInboundSmsCommand req, CancellationToken ct)
    {
        var s = req.Sms;

        var isMultipart =
            s.TotalParts is > 1 &&
            s.PartNumber is not null &&
            s.ConcatRef  is not null &&
            s.PartNumber >= 1 &&
            s.PartNumber <= s.TotalParts;

        await using var db = await _dbContextFactory.CreateAsync(ct);

        return isMultipart
            ? await PersistPartAsync(db, s, ct)
            : await PersistSingleAsync(db, s, ct);
    }

    // ------------------------------------------------------------------
    // Fast path — unchanged behaviour for single-part SMS.
    // ------------------------------------------------------------------
    private async Task<Result<bool>> PersistSingleAsync(
        IApplicationDbContext db, InboundSmsDto s, CancellationToken ct)
    {
        var already = await db.SmsMessages.AnyAsync(x =>
            x.Direction == SmsDirection.Inbound &&
            x.GatewayId == s.GatewayId.ToString(), ct);
        if (already)
        {
            _log.LogDebug("Inbound SMS gatewayId={Id} already stored; skipping.", s.GatewayId);
            return await Result<bool>.SuccessAsync(false);
        }

        var entity = BuildEntity(s, s.Body, s.GatewayId);

        try
        {
            entity.AddDomainEvent(new SmsMessageCreatedEvent(entity));
            db.SmsMessages.Add(entity);
            await db.SaveChangesAsync(ct);
        }
        catch (DbUpdateException)
        {
            _log.LogDebug(
                "Concurrent inbound insert for gatewayId={Id}; duplicate.", s.GatewayId);
            return await Result<bool>.SuccessAsync(false);
        }

        _log.LogInformation("Inbound SMS stored: gatewayId={Id} from={From}",
                            s.GatewayId, s.From);

        await PublishAsync(entity, s, ct);
        return await Result<bool>.SuccessAsync(true);
    }

    // ------------------------------------------------------------------
    // Multipart path — stage, then assemble once all parts are present.
    // ------------------------------------------------------------------
    private async Task<Result<bool>> PersistPartAsync(
        IApplicationDbContext db, InboundSmsDto s, CancellationToken ct)
    {
        var refNum = s.ConcatRef!.Value;
        var part   = s.PartNumber!.Value;
        var total  = s.TotalParts!.Value;
        var cutoff = DateTime.UtcNow - AssemblyWindow;

        // (1) Insert this part unless the exact (From, ConcatRef, Part) already
        //     exists within the window. The unique index is the real guard;
        //     this check just avoids the exception on the common path.
        var duplicate = await db.SmsInboundParts.AnyAsync(p =>
            p.From      == s.From &&
            p.ConcatRef == refNum &&
            p.PartNumber == part &&
            p.CreatedAt >= cutoff, ct);

        if (duplicate)
        {
            _log.LogDebug(
                "Duplicate part {Part}/{Total} ref={Ref} from={From}; skipping.",
                part, total, refNum, s.From);
            return await Result<bool>.SuccessAsync(false);
        }

        db.SmsInboundParts.Add(new SmsInboundPart
        {
            From       = s.From,
            ConcatRef  = refNum,
            PartNumber = part,
            TotalParts = total,
            Body       = s.Body,
            GatewayId  = s.GatewayId,
            Ts         = s.Ts,
            SimSlot    = s.SimSlot ?? 0,
            CreatedAt  = DateTime.UtcNow,
        });

        try { await db.SaveChangesAsync(ct); }
        catch (DbUpdateException)
        {
            _log.LogDebug(
                "Concurrent insert of part {Part}/{Total} ref={Ref}; duplicate.",
                part, total, refNum);
            return await Result<bool>.SuccessAsync(false);
        }

        // (2) Load every part of this group that's still within the window.
        var parts = await LoadGroupAsync(db, s.From, refNum, cutoff, ct);

        if (parts.Count < total)
        {
            // Grace: a sibling webhook/poll thread may be committing right now.
            await Task.Delay(RaceGrace, ct);
            parts = await LoadGroupAsync(db, s.From, refNum, cutoff, ct);
        }

        if (parts.Count < total)
        {
            _log.LogDebug(
                "Part {Part}/{Total} ref={Ref} stored; waiting for more.",
                part, total, refNum);
            return await Result<bool>.SuccessAsync(true);
        }

        // (3) Another thread may have already assembled this group.
        if (parts.Any(p => p.AssembledSmsMessageId is not null))
            return await Result<bool>.SuccessAsync(false);

        var ordered = parts.OrderBy(p => p.PartNumber).ToList();
        var body    = string.Concat(ordered.Select(p => p.Body));
        var first   = ordered[0];

        // Use the last-arriving GatewayId so redelivery of any single part is
        // still deduped against the SmsMessages.GatewayId unique index.
        var entity = BuildEntity(s, body, s.GatewayId, first);

        try
        {
            entity.AddDomainEvent(new SmsMessageCreatedEvent(entity));
            db.SmsMessages.Add(entity);
            await db.SaveChangesAsync(ct);

            foreach (var p in ordered)
                p.AssembledSmsMessageId = entity.Id;

            await db.SaveChangesAsync(ct);
        }
        catch (DbUpdateException)
        {
            _log.LogDebug(
                "Concurrent assembly ref={Ref} from={From}; duplicate.",
                refNum, s.From);
            return await Result<bool>.SuccessAsync(false);
        }

        _log.LogInformation(
            "Multipart SMS assembled: ref={Ref} from={From} parts={N} gatewayIds=[{Ids}]",
            refNum, s.From, ordered.Count,
            string.Join(",", ordered.Select(p => p.GatewayId)));

        await PublishAsync(entity, s, ct);
        return await Result<bool>.SuccessAsync(true);
    }

    // ------------------------------------------------------------------
    // Helpers
    // ------------------------------------------------------------------

    private static Task<List<SmsInboundPart>> LoadGroupAsync(
        IApplicationDbContext db, string from, int refNum, DateTime cutoff,
        CancellationToken ct) =>
        db.SmsInboundParts
          .Where(p => p.From == from &&
                      p.ConcatRef == refNum &&
                      p.CreatedAt >= cutoff)
          .ToListAsync(ct);

    private static SmsMessage BuildEntity(
        InboundSmsDto s, string body, long gatewayIdForRow,
        SmsInboundPart? template = null) => new()
    {
        Direction  = SmsDirection.Inbound,
        From       = s.From,
        To         = "self",
        Body       = body,
        GatewayId  = gatewayIdForRow.ToString(),
        SimSlot    = s.SimSlot ?? template?.SimSlot ?? 0,
        ReceivedAt = DateTimeOffset.FromUnixTimeMilliseconds(
                        template?.Ts ?? s.Ts).UtcDateTime,
        Status     = SmsStatus.Delivered,
    };

    private async Task PublishAsync(
        SmsMessage entity, InboundSmsDto s, CancellationToken ct)
    {
        await _hub.SmsReceived(new SmsReceivedPayload(
            Id:         entity.Id,
            GatewayId:  s.GatewayId,
            From:       s.From,
            Body:       entity.Body,
            Ts:         s.Ts,
            ReceivedAt: (DateTime)entity.ReceivedAt));
    }
}