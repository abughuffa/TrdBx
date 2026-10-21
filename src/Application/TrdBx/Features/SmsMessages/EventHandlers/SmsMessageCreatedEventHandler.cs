using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.SmsMessages.EventHandlers;

public class SmsMessageCreatedEventHandler : INotificationHandler<SmsMessageCreatedEvent>
{
    private readonly ILogger<SmsMessageCreatedEventHandler> _logger;

    public SmsMessageCreatedEventHandler(
        ILogger<SmsMessageCreatedEventHandler> logger
        )
    {
        _logger = logger;
    }
    public ValueTask Handle(SmsMessageCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
        return ValueTask.CompletedTask;
    }
}
