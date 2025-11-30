using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.EventHandlers;

    public class TrackingUnitDeletedEventHandler : INotificationHandler<TrackingUnitDeletedEvent>
    {
        private readonly ILogger<TrackingUnitDeletedEventHandler> _logger;

        public TrackingUnitDeletedEventHandler(
            ILogger<TrackingUnitDeletedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(TrackingUnitDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
