
namespace CleanArchitecture.Blazor.Application.Features.Warehouses.EventHandlers;

public class WarehouseCreatedEventHandler : INotificationHandler<WarehouseCreatedEvent>
{
        private readonly ILogger<WarehouseCreatedEventHandler> _logger;

        public WarehouseCreatedEventHandler(
            ILogger<WarehouseCreatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(WarehouseCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
}
