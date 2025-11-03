using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.EventHandlers;

public class TrackingUnitCreatedEventHandler : INotificationHandler<TrackingUnitCreatedEvent>
{
        private readonly ILogger<TrackingUnitCreatedEventHandler> _logger;

        public TrackingUnitCreatedEventHandler(
            ILogger<TrackingUnitCreatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(TrackingUnitCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
}
