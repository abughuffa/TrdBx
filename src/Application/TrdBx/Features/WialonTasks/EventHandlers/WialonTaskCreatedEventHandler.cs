using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.WialonTasks.EventHandlers;

public class WialonTaskCreatedEventHandler : INotificationHandler<WialonTaskCreatedEvent>
{
    private readonly ILogger<WialonTaskCreatedEventHandler> _logger;

    public WialonTaskCreatedEventHandler(
        ILogger<WialonTaskCreatedEventHandler> logger
        )
    {
        _logger = logger;
    }
    public Task Handle(WialonTaskCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
        return Task.CompletedTask;
    }
}
