
namespace CleanArchitecture.Blazor.Application.Features.VehicleTypes.EventHandlers;

public class VehicleTypeCreatedEventHandler : INotificationHandler<VehicleTypeCreatedEvent>
{
        private readonly ILogger<VehicleTypeCreatedEventHandler> _logger;

        public VehicleTypeCreatedEventHandler(
            ILogger<VehicleTypeCreatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(VehicleTypeCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
}
