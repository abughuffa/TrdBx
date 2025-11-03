using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.CusPrices.EventHandlers;

public class CusPriceDeletedEventHandler : INotificationHandler<CusPriceDeletedEvent>
{
    private readonly ILogger<CusPriceDeletedEventHandler> _logger;

    public CusPriceDeletedEventHandler(
        ILogger<CusPriceDeletedEventHandler> logger
        )
    {
        _logger = logger;
    }
    public Task Handle(CusPriceDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
        return Task.CompletedTask;
    }
}
