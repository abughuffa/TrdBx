using CleanArchitecture.Blazor.Application.Features.SmsConfigTemplates.Specifications;
using CleanArchitecture.Blazor.Application.Features.SmsMessages.DTOs;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Specifications;
using CleanArchitecture.Blazor.Domain.Enums;


namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.SendConfig;

public class SendConfigCommand : IRequest<Result<SendSmsResult>>
{
    public int Id { get; set; }
    public int SmsConfigTemplateId { get; set; }
    public int SimSlot { get; set; } = 0;

}



public class SendConfigCommandHandler
    : IRequestHandler<SendConfigCommand, Result<SendSmsResult>>
{
    private readonly ISmsGatewayClient _gateway;
    private readonly IApplicationDbContextFactory _dbFactory;
    private readonly IApplicationHubWrapper _hub;
    private readonly ILogger<SendConfigCommandHandler> _log;

        private readonly IObjectMapper _objectMapper;


    public SendConfigCommandHandler(
        ISmsGatewayClient gateway,
        IApplicationDbContextFactory dbFactory,
        IApplicationHubWrapper hub,
        ILogger<SendConfigCommandHandler> log,
                IObjectMapper objectMapper)
    {
        _gateway     = gateway;
        _dbFactory   = dbFactory;
        _hub         = hub;
        _log         = log;
        _objectMapper = objectMapper;

    }

    public async ValueTask<Result<SendSmsResult>> Handle(
        SendConfigCommand request, CancellationToken ct)
    {

    await using var db = await _dbFactory.CreateAsync(ct);

    var unit = await db.TrackingUnits.Include(u=>u.SimCard).ApplySpecification(new TrackingUnitByIdSpecification(request.Id))
    .FirstAsync(ct) ?? throw new NotFoundException($"TrackingUnit with id: [{request.Id}] not found.");

    if (unit.SimCardId == null) 
        return await Result<SendSmsResult>.FailureAsync("Install SimCard to the TrackingUnit first.");

    var smsTemplate = await db.SmsConfigTemplates.ApplySpecification(new SmsConfigTemplateByIdSpecification(request.SmsConfigTemplateId))
    .FirstAsync(ct) ?? throw new NotFoundException($"SmsConfigTemplate with id: [{request.SmsConfigTemplateId}] not found.");

    var reference =  Guid.NewGuid().ToString("N");
    var to =  $"+218{unit?.SimCard!.SimCardNo}";
    var body = smsTemplate.SmsTemplate;


        var entity = new SmsMessageDto
        {
            Direction = SmsDirection.Outbound,
            To        = to,
            Body      = body,
            SimSlot   = request.SimSlot,
            Reference = reference,
            Status    = SmsStatus.Queued
        };

        // ── Phase 1: persist "Queued" ─────────────────────────────

            if (reference is not null)
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

        // ── Phase 2: call the gateway (no DbContext held) ─────────
        SendSmsResult result;

        try
        {
            result = await _gateway.SendAsync(to, body, request.SimSlot, reference, ct);
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