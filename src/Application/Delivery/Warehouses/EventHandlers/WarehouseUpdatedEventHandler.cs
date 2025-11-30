
namespace CleanArchitecture.Blazor.Application.Features.Warehouses.EventHandlers;

    public class WarehouseUpdatedEventHandler : INotificationHandler<WarehouseUpdatedEvent>
    {
        private readonly ILogger<WarehouseUpdatedEventHandler> _logger;

        public WarehouseUpdatedEventHandler(
            ILogger<WarehouseUpdatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(WarehouseUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
