
namespace CleanArchitecture.Blazor.Application.Features.Shipments.EventHandlers;

    public class ShipmentUpdatedEventHandler : INotificationHandler<ShipmentUpdatedEvent>
    {
        private readonly ILogger<ShipmentUpdatedEventHandler> _logger;

        public ShipmentUpdatedEventHandler(
            ILogger<ShipmentUpdatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(ShipmentUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
