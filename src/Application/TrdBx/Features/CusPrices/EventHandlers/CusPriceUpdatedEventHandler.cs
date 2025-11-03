using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.CusPrices.EventHandlers;

public class CusPriceUpdatedEventHandler : INotificationHandler<CusPriceUpdatedEvent>
{
    private readonly ILogger<CusPriceUpdatedEventHandler> _logger;

    public CusPriceUpdatedEventHandler(
        ILogger<CusPriceUpdatedEventHandler> logger
        )
    {
        _logger = logger;
    }
    public Task Handle(CusPriceUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
        return Task.CompletedTask;
    }
}
