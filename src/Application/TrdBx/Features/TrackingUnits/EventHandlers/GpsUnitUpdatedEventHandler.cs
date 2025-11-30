using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.EventHandlers;

    public class TrackingUnitUpdatedEventHandler : INotificationHandler<TrackingUnitUpdatedEvent>
    {
        private readonly ILogger<TrackingUnitUpdatedEventHandler> _logger;

        public TrackingUnitUpdatedEventHandler(
            ILogger<TrackingUnitUpdatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(TrackingUnitUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
