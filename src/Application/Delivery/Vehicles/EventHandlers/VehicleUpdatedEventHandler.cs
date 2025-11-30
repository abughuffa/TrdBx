
namespace CleanArchitecture.Blazor.Application.Features.Vehicles.EventHandlers;

    public class VehicleUpdatedEventHandler : INotificationHandler<VehicleUpdatedEvent>
    {
        private readonly ILogger<VehicleUpdatedEventHandler> _logger;

        public VehicleUpdatedEventHandler(
            ILogger<VehicleUpdatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(VehicleUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
