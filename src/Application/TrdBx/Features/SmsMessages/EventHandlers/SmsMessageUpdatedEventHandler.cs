using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.SmsMessages.EventHandlers;

public class SmsMessageUpdatedEventHandler : INotificationHandler<SmsMessageUpdatedEvent>
{
    private readonly ILogger<SmsMessageUpdatedEventHandler> _logger;

    public SmsMessageUpdatedEventHandler(
        ILogger<SmsMessageUpdatedEventHandler> logger
        )
    {
        _logger = logger;
    }
    public ValueTask Handle(SmsMessageUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
        return ValueTask.CompletedTask;
    }
}
