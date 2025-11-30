
namespace CleanArchitecture.Blazor.Application.Features.Warehouses.EventHandlers;

    public class WarehouseDeletedEventHandler : INotificationHandler<WarehouseDeletedEvent>
    {
        private readonly ILogger<WarehouseDeletedEventHandler> _logger;

        public WarehouseDeletedEventHandler(
            ILogger<WarehouseDeletedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(WarehouseDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
