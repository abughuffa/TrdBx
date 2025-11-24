
namespace CleanArchitecture.Blazor.Application.Features.Shipments.EventHandlers;

public class ShipmentCreatedEventHandler : INotificationHandler<ShipmentCreatedEvent>
{
        private readonly ILogger<ShipmentCreatedEventHandler> _logger;

        public ShipmentCreatedEventHandler(
            ILogger<ShipmentCreatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(ShipmentCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
}
