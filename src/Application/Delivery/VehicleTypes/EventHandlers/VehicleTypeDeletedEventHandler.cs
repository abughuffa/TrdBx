
namespace CleanArchitecture.Blazor.Application.Features.VehicleTypes.EventHandlers;

    public class VehicleTypeDeletedEventHandler : INotificationHandler<VehicleTypeDeletedEvent>
    {
        private readonly ILogger<VehicleTypeDeletedEventHandler> _logger;

        public VehicleTypeDeletedEventHandler(
            ILogger<VehicleTypeDeletedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(VehicleTypeDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
