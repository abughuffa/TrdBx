using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.WialonTasks.EventHandlers;

public class WialonTaskDeletedEventHandler : INotificationHandler<WialonTaskDeletedEvent>
{
    private readonly ILogger<WialonTaskDeletedEventHandler> _logger;

    public WialonTaskDeletedEventHandler(
        ILogger<WialonTaskDeletedEventHandler> logger
        )
    {
        _logger = logger;
    }
    public Task Handle(WialonTaskDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
        return Task.CompletedTask;
    }
}
