
namespace CleanArchitecture.Blazor.Application.Features.POIs.EventHandlers;

public class POICreatedEventHandler : INotificationHandler<POICreatedEvent>
{
        private readonly ILogger<POICreatedEventHandler> _logger;

        public POICreatedEventHandler(
            ILogger<POICreatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(POICreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
}
