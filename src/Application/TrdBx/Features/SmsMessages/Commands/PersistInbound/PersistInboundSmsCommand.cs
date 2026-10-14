
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
    private static readonly TimeSpan AssemblyWindow = TimeSpan.FromMinutes(10);
    private static readonly TimeSpan RaceGrace      = TimeSpan.FromMilliseconds(150);

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

    // ───────────────────────────────────────────────────────────
    // Single-part: unchanged.
    // ───────────────────────────────────────────────────────────

    private async Task<Result<bool>> PersistSingleAsync(
        IApplicationDbContext db, InboundSmsDto s, CancellationToken ct)
    {
        var already = await db.SmsMessages.AnyAsync(x =>
            x.Direction == SmsDirection.Inbound &&
            x.GatewayId == s.GatewayId.ToString(), ct);

        if (already) return await Result<bool>.SuccessAsync(false);

        var entity = new SmsMessage
        {
            Direction  = SmsDirection.Inbound,
            From       = s.From,
            To         = "self",
            Body       = s.Body,
            GatewayId  = s.GatewayId.ToString(),
            SimSlot    = s.SimSlot ?? 0,
            ReceivedAt = DateTimeOffset.FromUnixTimeMilliseconds(s.Ts).UtcDateTime,
            Status     = SmsStatus.Delivered,
        };

        try
        {
            entity.AddDomainEvent(new SmsMessageCreatedEvent(entity));
            db.SmsMessages.Add(entity);
            await db.SaveChangesAsync(ct);
        }
        catch (DbUpdateException)
        {
            return await Result<bool>.SuccessAsync(false);   // lost the race
        }

        await PublishAsync(entity, s, ct);
        return await Result<bool>.SuccessAsync(true);
    }

    // ───────────────────────────────────────────────────────────
    // Multipart: stage → claim → attach.
    // ───────────────────────────────────────────────────────────

    private async Task<Result<bool>> PersistPartAsync(
        IApplicationDbContext db, InboundSmsDto s, CancellationToken ct)
    {
        var refNum = s.ConcatRef!.Value;
        var part   = s.PartNumber!.Value;
        var total  = s.TotalParts!.Value;

        // ── 1. Have we already seen this exact part? No time filter:
        //       the parts table is the durable dedupe layer.
        if (await db.SmsInboundParts.AnyAsync(p =>
                p.From       == s.From &&
                p.ConcatRef  == refNum &&
                p.PartNumber == part &&
                p.TotalParts == total, ct))
        {
            _log.LogDebug(
                "Duplicate part {Part}/{Total} ref={Ref} from={From}; skipping.",
                part, total, refNum, s.From);
            return await Result<bool>.SuccessAsync(false);
        }

        // ── 2. Is any part with this GatewayId already inside an
        //       assembled SmsMessage? (Late redelivery of a part that
        //       arrived before pruning, but whose group is done.)
        var token = $"|{s.GatewayId}|";
        if (await db.SmsMessages.AnyAsync(m =>
                m.Direction == SmsDirection.Inbound &&
                m.PartsGatewayIds != null &&
                m.PartsGatewayIds.Contains(token), ct))
        {
            _log.LogDebug(
                "Part GatewayId={Id} already attached to an assembled SMS; skipping.",
                s.GatewayId);
            return await Result<bool>.SuccessAsync(false);
        }

        // ── 3. Stage the part. Unique index is the real guard; the
        //       AnyAsync above just avoids the exception on the happy path.
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

        // ── 4. Load the group. Window keeps a stale (ref-reused) group
        //       from colliding with this fresh one; AssembledSmsMessageId
        //       filter skips groups already committed.
        var cutoff = DateTime.UtcNow - AssemblyWindow;

        var parts = await db.SmsInboundParts
            .Where(p => p.From       == s.From &&
                        p.ConcatRef  == refNum &&
                        p.TotalParts == total &&
                        p.AssembledSmsMessageId == null &&
                        p.CreatedAt  >= cutoff)
            .OrderBy(p => p.PartNumber)
            .ToListAsync(ct);

        if (parts.Count < total)
        {
            // Give a sibling transaction a chance to commit.
            await Task.Delay(RaceGrace, ct);

            parts = await db.SmsInboundParts
                .Where(p => p.From       == s.From &&
                            p.ConcatRef  == refNum &&
                            p.TotalParts == total &&
                            p.AssembledSmsMessageId == null &&
                            p.CreatedAt  >= cutoff)
                .OrderBy(p => p.PartNumber)
                .ToListAsync(ct);
        }

        if (parts.Count < total)
        {
            _log.LogDebug(
                "Part {Part}/{Total} ref={Ref} stored; waiting for {Missing} more.",
                part, total, refNum, total - parts.Count);
            return await Result<bool>.SuccessAsync(true);
        }

        // ── 5. Sanity check: parts must be 1..total, no gaps.
        var seen = parts.Select(p => p.PartNumber).ToHashSet();
        if (seen.Count != total || !Enumerable.Range(1, total).All(seen.Contains))
        {
            _log.LogWarning(
                "Group ref={Ref} from={From} has {N} parts but positions are {Positions}; deferring.",
                refNum, s.From, parts.Count,
                string.Join(",", seen.OrderBy(x => x)));
            return await Result<bool>.SuccessAsync(true);
        }

        // ── 6. Assemble. Unique index on AssemblyKey resolves the race:
        //       exactly one thread wins, everyone else catches DbUpdateException.
        var ordered  = parts.OrderBy(p => p.PartNumber).ToList();
        var body     = string.Concat(ordered.Select(p => p.Body));
        var first    = ordered[0];
        var assembly = $"{s.From}|{refNum}|{total}";

        var entity = new SmsMessage
        {
            Direction       = SmsDirection.Inbound,
            From            = s.From,
            To              = "self",
            Body            = body,
            // Store first part's id as the canonical GatewayId (human-friendly),
            // and every part's id in PartsGatewayIds for dedupe.
            GatewayId       = first.GatewayId.ToString(),
            PartsGatewayIds = "|" + string.Join("|", ordered.Select(p => p.GatewayId)) + "|",
            AssemblyKey     = assembly,
            IsMultipart     = true,
            ConcatRef       = refNum,
            TotalParts      = total,
            SimSlot         = s.SimSlot ?? first.SimSlot,
            ReceivedAt      = DateTimeOffset.FromUnixTimeMilliseconds(first.Ts).UtcDateTime,
            Status          = SmsStatus.Delivered,
        };

        try
        {
            entity.AddDomainEvent(new SmsMessageCreatedEvent(entity));
            db.SmsMessages.Add(entity);
            await db.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
        {
            _log.LogDebug(ex,
                "Concurrent assembly of ref={Ref} from={From}; another thread won.",
                refNum, s.From);
            return await Result<bool>.SuccessAsync(false);
        }

        // ── 7. Attach parts to the new SmsMessage. This is bookkeeping;
        //       if it fails the next redelivery is still caught by (2).
        try
        {
            foreach (var p in ordered) p.AssembledSmsMessageId = entity.Id;
            await db.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
        {
            _log.LogWarning(ex,
                "Failed to back-link parts to SmsMessage {Id}; harmless.", entity.Id);
        }

        _log.LogInformation(
            "Multipart SMS assembled: id={Id} ref={Ref} from={From} parts={N}",
            entity.Id, refNum, s.From, ordered.Count);

        await PublishAsync(entity, s, ct);
        return await Result<bool>.SuccessAsync(true);
    }

    private async Task PublishAsync(
        SmsMessage entity, InboundSmsDto s, CancellationToken ct)
    {
        await _hub.SmsReceived(new SmsReceivedPayload(
            Id:         entity.Id,
            GatewayId:  long.Parse(entity.GatewayId!),  // first part's id
            From:       s.From,
            Body:       entity.Body,
            Ts:         s.Ts,
            ReceivedAt: (DateTime)entity.ReceivedAt));
    }
}