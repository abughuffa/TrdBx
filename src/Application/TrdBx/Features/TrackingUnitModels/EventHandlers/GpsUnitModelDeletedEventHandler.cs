using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.TrackingUnitModels.EventHandlers;

    public class TrackingUnitModelDeletedEventHandler : INotificationHandler<TrackingUnitModelDeletedEvent>
    {
        private readonly ILogger<TrackingUnitModelDeletedEventHandler> _logger;

        public TrackingUnitModelDeletedEventHandler(
            ILogger<TrackingUnitModelDeletedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(TrackingUnitModelDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
