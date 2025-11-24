
namespace CleanArchitecture.Blazor.Application.Features.Shipments.EventHandlers;

    public class ShipmentDeletedEventHandler : INotificationHandler<ShipmentDeletedEvent>
    {
        private readonly ILogger<ShipmentDeletedEventHandler> _logger;

        public ShipmentDeletedEventHandler(
            ILogger<ShipmentDeletedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(ShipmentDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
