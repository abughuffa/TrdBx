
using CleanArchitecture.Blazor.Application.Features.SmsMessages.DTOs;
using CleanArchitecture.Blazor.Domain.Enums;


namespace CleanArchitecture.Blazor.Application.Features.SmsMessages.Commands.Resend;

public class ResendSmsCommand : IRequest<Result<SendSmsResult>>
{
    /// <summary>Primary key of the SmsMessage to resend.</summary>
    public int Id { get; set; }

    /// <summary>
    /// Optional override. If null, the original message body is used.
    /// Useful when you want to fix a typo and retry.
    /// </summary>
    public string? OverrideBody { get; set; }

    /// <summary>
    /// Optional override. If null, the original recipient is used.
    /// </summary>
    public string? OverrideTo { get; set; }

    /// <summary>
    /// Optional override. If null, the original SIM slot is used.
    /// </summary>
    public int? OverrideSimSlot { get; set; }
}

public class ResendSmsCommandHandler
    : IRequestHandler<ResendSmsCommand, Result<SendSmsResult>>
{
    private readonly ISmsGatewayClient _gateway;
    private readonly IApplicationDbContextFactory _dbFactory;
    private readonly IApplicationHubWrapper _hub;
    private readonly ILogger<ResendSmsCommandHandler> _log;

    public ResendSmsCommandHandler(
        ISmsGatewayClient gateway,
        IApplicationDbContextFactory dbFactory,
        IApplicationHubWrapper hub,
        ILogger<ResendSmsCommandHandler> log)
    {
        _gateway     = gateway;
        _dbFactory   = dbFactory;
        _hub         = hub;
        _log         = log;
    }

    public async ValueTask<Result<SendSmsResult>> Handle(
        ResendSmsCommand req, CancellationToken ct)
    {
        // ──────────────────────────────────────────────────────────
        // Pre-flight: load the original row, verify it's resendable,
        // and (optionally) rewrite its reference so the retry is
        // independently addressable in the gateway and in our DB.
        // ──────────────────────────────────────────────────────────
        SmsMessage original;
        await using (var db = await _dbFactory.CreateAsync(ct))
        {
            original = await db.SmsMessages
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == req.Id, ct)
                ?? null!;

            if (original is null)
                return await Result<SendSmsResult>.FailureAsync(
                    $"SMS id {req.Id} not found.");

            // if (original.Direction != SmsDirection.Outbound)
            //     return await Result<SendSmsResult>.FailureAsync(
            //         $"SMS id {req.Id} is not an outbound message; only outbound can be resent.");

            if (original.Status is not (SmsStatus.Failed or SmsStatus.Unknown))
                return await Result<SendSmsResult>.FailureAsync(
                    $"SMS id {req.Id} has status '{original.Status}'; only Failed or Unknown can be resent.");

        }

        var to        = req.OverrideTo        ?? original.To;
        var body      = req.OverrideBody      ?? original.Body;
        var simSlot   = req.OverrideSimSlot   ?? original.SimSlot;

        // Fresh reference: the previous one is tied to the failed attempt.
        // Keeping them distinct makes retries visible in both the gateway
        // log and our own audit trail.
        var newReference = Guid.NewGuid().ToString("N");

        // ──────────────────────────────────────────────────────────
        // Phase 1' — update the row to Queued *before* calling the
        // gateway, so the UI immediately reflects "in flight" and
        // concurrent resends can't both fire for the same row.
        // ──────────────────────────────────────────────────────────
        await using (var db = await _dbFactory.CreateAsync(ct))
        {
            var tracked = await db.SmsMessages.FirstOrDefaultAsync(x => x.Id == req.Id, ct);
            if (tracked is null)
                return await Result<SendSmsResult>.FailureAsync(
                    $"SMS id {req.Id} disappeared between reads.");

            // Guard against two simultaneous resend clicks.
            if (tracked.Status == SmsStatus.Queued)
                return await Result<SendSmsResult>.FailureAsync(
                    $"SMS id {req.Id} is already queued for sending.");

            tracked.Status    = SmsStatus.Queued;
            tracked.Error     = null;
            tracked.To        = to;
            tracked.Body      = body;
            tracked.SimSlot   = simSlot;
            tracked.Reference = newReference;
            tracked.GatewayId = null;

            tracked.AddDomainEvent(new SmsMessageUpdatedEvent(tracked));
            await db.SaveChangesAsync(ct);
        }

        // ──────────────────────────────────────────────────────────
        // Phase 2' — call the gateway (no DbContext held).
        // Same shape as SendAndPersistSmsCommandHandler.
        // ──────────────────────────────────────────────────────────
        SendSmsResult result;
        SmsStatus finalStatus;
        string? finalError;
        DateTime? sentAt = null;

        try
        {
            result = await _gateway.SendAsync(to, body, simSlot, newReference, ct);
            finalStatus = MapStatus(result.Status);
            finalError  = result.Error;
            if (finalStatus is SmsStatus.Sent or SmsStatus.Delivered)
                sentAt = DateTime.UtcNow;
        }
        catch (Exception ex)
        {
            _log.LogError(ex,
                "Resend failed for SMS id={Id} newReference={Reference}",
                req.Id, newReference);

            finalStatus = SmsStatus.Failed;
            finalError  = ex.Message;

            await PersistFinalStateAsync(req.Id, finalStatus, finalError, null, null, ct);
            await SafeBroadcastAsync(req.Id, newReference, finalStatus, finalError, null, null);

            return await Result<SendSmsResult>.FailureAsync(ex.Message);
        }

        // ──────────────────────────────────────────────────────────
        // Phase 3' — persist the final state.
        // ──────────────────────────────────────────────────────────
        await PersistFinalStateAsync(req.Id, finalStatus, finalError, sentAt, result, ct);
        await SafeBroadcastAsync(req.Id, newReference, finalStatus, finalError, sentAt, null);

        return await Result<SendSmsResult>.SuccessAsync(result);
    }

    private async Task PersistFinalStateAsync(
        int id, SmsStatus status, string? error, DateTime? sentAt,
        SendSmsResult? result, CancellationToken ct)
    {
        try
        {
            await using var db = await _dbFactory.CreateAsync(ct);
            var entity = await db.SmsMessages.FindAsync(new object?[] { id }, ct);
            if (entity is null)
            {
                _log.LogWarning(
                    "SmsMessage id={Id} not found while persisting resend outcome.", id);
                return;
            }

            entity.Status    = status;
            entity.Error     = error;
            entity.SentAt    = sentAt;
            entity.GatewayId = result?.Id ?? entity.GatewayId;

            entity.AddDomainEvent(new SmsMessageUpdatedEvent(entity));
            await db.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            _log.LogWarning(ex,
                "Failed to persist resend outcome for SMS id={Id}", id);
        }
    }

    private async Task SafeBroadcastAsync(
        int id, string reference, SmsStatus status, string? error,
        DateTime? sentAt, DateTime? deliveredAt)
    {
        try
        {
            await _hub.SmsStatusChanged(new SmsStatusChangedPayload(
                Id: id, Reference: reference, Status: status.ToString(),
                Error: error, SentAt: sentAt, DeliveredAt: deliveredAt));
        }
        catch (Exception ex)
        {
            _log.LogWarning(ex,
                "Broadcast SmsStatusChanged failed for SMS id={Id}", id);
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