
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
        await using (var db = await _dbFactory.CreateAsync(ct))
        {
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

            entity.Id = item.Id;

        }

        // ── Phase 2: call the gateway (no DbContext held) ─────────
        SendSmsResult result;
        try
        {
            result = await _gateway.SendAsync(req.To, req.Body, req.SimSlot, reference, ct);
            entity.GatewayId = result.Id;
            entity.Status    = MapStatus(result.Status);
            entity.Error     = result.Error;
            if (entity.Status is SmsStatus.Sent or SmsStatus.Delivered)
                entity.SentAt = DateTime.UtcNow;
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "SMS send failed for reference={Reference}", reference);
            entity.Status = SmsStatus.Failed;
            entity.Error  = ex.Message;

            await PersistUpdateAsync(entity, ct);
            await SafeBroadcastAsync(entity);

            return await Result<SendSmsResult>.FailureAsync(ex.Message);
        }

        // ── Phase 3: persist the final state ──────────────────────
        await PersistUpdateAsync(entity, ct);
        await SafeBroadcastAsync(entity);

        return await Result<SendSmsResult>.SuccessAsync(result);
    }

private async Task PersistUpdateAsync(SmsMessageDto entity, CancellationToken ct)
{
    try
    {
        await using var db = await _dbFactory.CreateAsync(ct);
        var item = await db.SmsMessages.FindAsync(entity.Id, ct);
        if (item is null)
        {
            _log.LogWarning(
                "SmsMessage id={Id} not found while persisting final state.", entity.Id);
            return;
        }

        item.Status       = entity.Status;
        item.Error        = entity.Error;
        item.GatewayId    = entity.GatewayId;
        item.SentAt       = entity.SentAt;
        item.DeliveredAt  = entity.DeliveredAt;

        item.AddDomainEvent(new SmsMessageUpdatedEvent(item));
        await db.SaveChangesAsync(ct);
    }
    catch (Exception ex)
    {
        _log.LogError(ex,
            "Failed to persist final state for reference={Reference}", entity.Reference);
    }
}


    private async Task SafeBroadcastAsync(SmsMessageDto entity)
    {
        try
        {
            await _hub.SmsStatusChanged(new SmsStatusChangedPayload(
                Id:          entity.Id,
                Reference:   entity.Reference ?? string.Empty,
                Status:      entity.Status.ToString(),
                Error:       entity.Error,
                SentAt:      entity.SentAt,
                DeliveredAt: entity.DeliveredAt));
        }
        catch (Exception ex)
        {
            _log.LogWarning(ex,
                "Broadcast SmsStatusChanged failed for reference={Reference}", entity.Reference);
        }
    }

    private static SmsStatus MapStatus(string status) => status switch
    {
        "queued" => SmsStatus.Queued,
        "sent"   => SmsStatus.Sent,
        "failed" => SmsStatus.Failed,
        _        => SmsStatus.Unknown
    };
}