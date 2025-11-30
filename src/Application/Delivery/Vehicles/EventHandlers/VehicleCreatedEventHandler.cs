
namespace CleanArchitecture.Blazor.Application.Features.Vehicles.EventHandlers;

public class VehicleCreatedEventHandler : INotificationHandler<VehicleCreatedEvent>
{
        private readonly ILogger<VehicleCreatedEventHandler> _logger;

        public VehicleCreatedEventHandler(
            ILogger<VehicleCreatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(VehicleCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
}
