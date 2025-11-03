using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.TrackingUnitModels.EventHandlers;

    public class TrackingUnitModelUpdatedEventHandler : INotificationHandler<TrackingUnitModelUpdatedEvent>
    {
        private readonly ILogger<TrackingUnitModelUpdatedEventHandler> _logger;

        public TrackingUnitModelUpdatedEventHandler(
            ILogger<TrackingUnitModelUpdatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(TrackingUnitModelUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
