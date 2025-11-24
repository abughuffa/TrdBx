
namespace CleanArchitecture.Blazor.Application.Features.VehicleTypes.EventHandlers;

    public class VehicleTypeUpdatedEventHandler : INotificationHandler<VehicleTypeUpdatedEvent>
    {
        private readonly ILogger<VehicleTypeUpdatedEventHandler> _logger;

        public VehicleTypeUpdatedEventHandler(
            ILogger<VehicleTypeUpdatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(VehicleTypeUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
