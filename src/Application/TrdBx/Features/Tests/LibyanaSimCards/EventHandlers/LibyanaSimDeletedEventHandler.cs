using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.LibyanaSimCards.EventHandlers;

public class LibyanaSimCardDeletedEventHandler : INotificationHandler<LibyanaSimCardDeletedEvent>
{
    private readonly ILogger<LibyanaSimCardDeletedEventHandler> _logger;

    public LibyanaSimCardDeletedEventHandler(
        ILogger<LibyanaSimCardDeletedEventHandler> logger
        )
    {
        _logger = logger;
    }
    public Task Handle(LibyanaSimCardDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
        return Task.CompletedTask;
    }
}