using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.TrackingUnitModels.EventHandlers;

public class TrackingUnitModelCreatedEventHandler : INotificationHandler<TrackingUnitModelCreatedEvent>
{
        private readonly ILogger<TrackingUnitModelCreatedEventHandler> _logger;

        public TrackingUnitModelCreatedEventHandler(
            ILogger<TrackingUnitModelCreatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(TrackingUnitModelCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
}
