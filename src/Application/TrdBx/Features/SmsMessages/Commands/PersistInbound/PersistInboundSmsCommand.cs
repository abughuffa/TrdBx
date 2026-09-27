
using CleanArchitecture.Blazor.Application.Features.SmsMessages.DTOs;
using CleanArchitecture.Blazor.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace CleanArchitecture.Blazor.Application.Features.SmsMessages.Commands.PersistSms;

public record PersistInboundSmsCommand(InboundSmsDto Sms) : IRequest<Result<bool>>;

public class PersistInboundSmsCommandHandler
    : IRequestHandler<PersistInboundSmsCommand, Result<bool>>
{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IApplicationHubWrapper _hub;
    private readonly ILogger<PersistInboundSmsCommandHandler> _log;

    public PersistInboundSmsCommandHandler(
        IApplicationDbContextFactory dbContextFactory,
        IApplicationHubWrapper hub,
        ILogger<PersistInboundSmsCommandHandler> log)
    {
        _dbContextFactory          = dbContextFactory;
        _hub         = hub;
        _log         = log;
    }

    public async ValueTask<Result<bool>> Handle(PersistInboundSmsCommand req, CancellationToken ct)
    {

        await using var _db = await _dbContextFactory.CreateAsync(ct);
        var s = req.Sms;

        // Idempotency: skip if this gateway id is already stored.

        var already = await _db.SmsMessages
            .AnyAsync(x => x.Direction == SmsDirection.Inbound
                        && x.GatewayId == s.GatewayId.ToString(), ct);
        if (already)
        {
            _log.LogDebug("Inbound SMS gatewayId={GatewayId} already stored; skipping.", s.GatewayId);
            return await Result<bool>.SuccessAsync(false);
        }

        var receivedAt = DateTimeOffset
            .FromUnixTimeMilliseconds(s.Ts)
            .UtcDateTime;

        var entity = new SmsMessage
        {
            Direction  = SmsDirection.Inbound,
            From       = s.From,
            To         = "self",
            Body       = s.Body,
            GatewayId  = s.GatewayId.ToString(),
            SimSlot    = s.SimSlot ?? 0,
            ReceivedAt = receivedAt,
            Status     = SmsStatus.Delivered
        };

        try
        {
            entity.AddDomainEvent(new SmsMessageCreatedEvent(entity));
            _db.SmsMessages.Add(entity);
            await _db.SaveChangesAsync(ct);
        }
        catch (DbUpdateException)
        {
            // Concurrent insert — unique index on GatewayId caught the race.
            _log.LogDebug(
                "Concurrent inbound insert for gatewayId={GatewayId}; treating as duplicate.",
                s.GatewayId);
            return await Result<bool>.SuccessAsync(false);
        }

        _log.LogInformation("Inbound SMS stored: gatewayId={GatewayId} from={From}",
                            s.GatewayId, s.From);

        await _hub.SmsReceived(new SmsReceivedPayload(
            Id:         entity.Id,
            GatewayId:  s.GatewayId,
            From:       s.From,
            Body:       s.Body,
            Ts:         s.Ts,
            ReceivedAt: receivedAt
            ));

        return await Result<bool>.SuccessAsync(true);
    }
}

