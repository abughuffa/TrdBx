// Lice

using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.LibyanaSimCards.EventHandlers;

public class LibyanaSimCardCreatedEventHandler : INotificationHandler<LibyanaSimCardCreatedEvent>
{
    private readonly ILogger<LibyanaSimCardCreatedEventHandler> _logger;

    public LibyanaSimCardCreatedEventHandler(
        ILogger<LibyanaSimCardCreatedEventHandler> logger
        )
    {
        _logger = logger;
    }
    public Task Handle(LibyanaSimCardCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
        return Task.CompletedTask;
    }
}
