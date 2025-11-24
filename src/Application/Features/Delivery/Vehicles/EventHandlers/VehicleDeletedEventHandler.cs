
namespace CleanArchitecture.Blazor.Application.Features.Vehicles.EventHandlers;

    public class VehicleDeletedEventHandler : INotificationHandler<VehicleDeletedEvent>
    {
        private readonly ILogger<VehicleDeletedEventHandler> _logger;

        public VehicleDeletedEventHandler(
            ILogger<VehicleDeletedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(VehicleDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
