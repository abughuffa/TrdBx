using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.LibyanaSimCards.EventHandlers;

public class LibyanaSimCardUpdatedEventHandler : INotificationHandler<LibyanaSimCardUpdatedEvent>
{
    private readonly ILogger<LibyanaSimCardUpdatedEventHandler> _logger;

    public LibyanaSimCardUpdatedEventHandler(
        ILogger<LibyanaSimCardUpdatedEventHandler> logger
        )
    {
        _logger = logger;
    }
    public Task Handle(LibyanaSimCardUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
        return Task.CompletedTask;
    }
}
