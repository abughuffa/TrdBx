
using System.Runtime.CompilerServices;
using CleanArchitecture.Blazor.Application.Features.SmsMessages.DTOs;
using CleanArchitecture.Blazor.Domain.Enums;


namespace CleanArchitecture.Blazor.Application.Features.SmsMessages.Commands.SendAndPersist;

public class SendAndPersistSmsCommand : IRequest<Result<SendSmsResult>>
{
    public string  To        { get; set; } = null!;
    public string  Body      { get; set; } = null!;
    public int     SimSlot   { get; set; } = 0;
    public string? Reference { get; set; }
}

// public class SendAndPersistSmsCommandValidator : AbstractValidator<SendAndPersistSmsCommand>
// {
//     public SendAndPersistSmsCommandValidator()
//     {
//         RuleFor(x => x.To).NotEmpty().MaximumLength(20);
//         RuleFor(x => x.Body).NotEmpty().MaximumLength(1600);
//         RuleFor(x => x.SimSlot).InclusiveBetween(0, 3);
//         RuleFor(x => x.Reference).MaximumLength(64).When(x => x.Reference is not null);
//     }
// }

public class SendAndPersistSmsCommandHandler
    : IRequestHandler<SendAndPersistSmsCommand, Result<SendSmsResult>>
{
    private readonly ISmsGatewayClient _gateway;
    private readonly IApplicationDbContextFactory _dbFactory;
    private readonly IApplicationHubWrapper _hub;
    private readonly ILogger<SendAndPersistSmsCommandHandler> _log;

        private readonly IObjectMapper _objectMapper;


    public SendAndPersistSmsCommandHandler(
        ISmsGatewayClient gateway,
        IApplicationDbContextFactory dbFactory,
        IApplicationHubWrapper hub,
        ILogger<SendAndPersistSmsCommandHandler> log,
                IObjectMapper objectMapper)
    {
        _gateway     = gateway;
        _dbFactory   = dbFactory;
        _hub         = hub;
        _log         = log;
                _objectMapper = objectMapper;

    }

    public async ValueTask<Result<SendSmsResult>> Handle(
        SendAndPersistSmsCommand req, CancellationToken ct)
    {
        var reference = string.IsNullOrWhiteSpace(req.Reference)
            ? Guid.NewGuid().ToString("N")
            : req.Reference!;

        var entity = new SmsMessageDto
        {
            Direction = SmsDirection.Outbound,
            To        = req.To,
            Body      = req.Body,
            SimSlot   = req.SimSlot,
            Reference = reference,
            Status    = SmsStatus.Queued
        };

        // ── Phase 1: persist "Queued" ─────────────────────────────
        //await using (var db = await _dbFactory.CreateAsync(ct));

     await using var db = await _dbFactory.CreateAsync(ct);

        
            if (req.Reference is not null)
            {
                var dup = await db.SmsMessages
                    .AnyAsync(x => x.Reference == reference, ct);
                if (dup)
                    return await Result<SendSmsResult>.FailureAsync(
                        $"Reference '{reference}' is already in use.");
            }

            var item = _objectMapper.Map<SmsMessage>(entity);


            item.AddDomainEvent(new SmsMessageCreatedEvent(item));
            db.SmsMessages.Add(item);
            await db.SaveChangesAsync(ct);

            //entity.Id = item.Id;

        

        // ── Phase 2: call the gateway (no DbContext held) ─────────
        SendSmsResult result;

        try
        {
            result = await _gateway.SendAsync(req.To, req.Body, req.SimSlot, reference, ct);
            item.GatewayId = result.Id;
            item.Status    = MapStatus(result.Status);
            item.Error     = result.Error;
            if (item.Status is SmsStatus.Sent or SmsStatus.Delivered)
                item.SentAt = DateTime.UtcNow;
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "SMS send failed for reference={Reference}", reference);
            item.Status = SmsStatus.Failed;
            item.Error  = ex.Message;

            await PersistUpdateAsync(db, item, ct);
            await SafeBroadcastAsync(item);

            return await Result<SendSmsResult>.FailureAsync(ex.Message);
        }

        // ── Phase 3: persist the final state ──────────────────────
        await PersistUpdateAsync(db, item, ct);
        await SafeBroadcastAsync(item);

        return await Result<SendSmsResult>.SuccessAsync(result);
    }

private async Task PersistUpdateAsync(IApplicationDbContext cnx ,SmsMessage onsentsms, CancellationToken ct)
{
    try
    {
        //var item = _objectMapper.Map<SmsMessage>(entity);

        onsentsms.AddDomainEvent(new SmsMessageUpdatedEvent(onsentsms));
        await cnx.SaveChangesAsync(ct);
    }
    catch (Exception ex)
    {
        _log.LogError(ex,
            "Failed to persist final state for reference={Reference}", onsentsms.Reference);
    }
}


    private async Task SafeBroadcastAsync(SmsMessage onsentsms)
    {
        try
        {
            await _hub.SmsStatusChanged(new SmsStatusChangedPayload(
                Id:          onsentsms.Id,
                Reference:   onsentsms.Reference ?? string.Empty,
                Status:      onsentsms.Status.ToString(),
                Error:       onsentsms.Error,
                SentAt:      onsentsms.SentAt,
                DeliveredAt: onsentsms.DeliveredAt));
        }
        catch (Exception ex)
        {
            _log.LogWarning(ex,
                "Broadcast SmsStatusChanged failed for reference={Reference}", onsentsms.Reference);
        }
    }

    private static SmsStatus MapStatus(string status) => status switch
    {
        "queued" => SmsStatus.Sent,
        "sent"   => SmsStatus.Sent,
        "failed" => SmsStatus.Failed,
        _        => SmsStatus.Unknown
    };
}