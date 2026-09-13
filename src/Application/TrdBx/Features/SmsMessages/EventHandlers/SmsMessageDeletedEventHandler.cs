using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.SmsMessages.EventHandlers;

public class SmsMessageDeletedEventHandler : INotificationHandler<SmsMessageDeletedEvent>
{
    private readonly ILogger<SmsMessageDeletedEventHandler> _logger;

    public SmsMessageDeletedEventHandler(
        ILogger<SmsMessageDeletedEventHandler> logger
        )
    {
        _logger = logger;
    }
    public ValueTask Handle(SmsMessageDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
        return ValueTask.CompletedTask;
    }
}
