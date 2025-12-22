
namespace CleanArchitecture.Blazor.Application.Features.POIs.EventHandlers;

    public class POIDeletedEventHandler : INotificationHandler<POIDeletedEvent>
    {
        private readonly ILogger<POIDeletedEventHandler> _logger;

        public POIDeletedEventHandler(
            ILogger<POIDeletedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(POIDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
