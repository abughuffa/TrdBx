using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.WialonTasks.EventHandlers;

public class WialonTaskUpdatedEventHandler : INotificationHandler<WialonTaskUpdatedEvent>
{
    private readonly ILogger<WialonTaskUpdatedEventHandler> _logger;

    public WialonTaskUpdatedEventHandler(
        ILogger<WialonTaskUpdatedEventHandler> logger
        )
    {
        _logger = logger;
    }
    public Task Handle(WialonTaskUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
        return Task.CompletedTask;
    }
}
