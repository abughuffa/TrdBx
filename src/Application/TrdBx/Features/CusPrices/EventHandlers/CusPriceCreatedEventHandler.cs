using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.CusPrices.EventHandlers;

public class CusPriceCreatedEventHandler : INotificationHandler<CusPriceCreatedEvent>
{
    private readonly ILogger<CusPriceCreatedEventHandler> _logger;

    public CusPriceCreatedEventHandler(
        ILogger<CusPriceCreatedEventHandler> logger
        )
    {
        _logger = logger;
    }
    public Task Handle(CusPriceCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
        return Task.CompletedTask;
    }
}
