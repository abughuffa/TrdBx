
namespace CleanArchitecture.Blazor.Application.Features.POIs.EventHandlers;

    public class POIUpdatedEventHandler : INotificationHandler<POIUpdatedEvent>
    {
        private readonly ILogger<POIUpdatedEventHandler> _logger;

        public POIUpdatedEventHandler(
            ILogger<POIUpdatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(POIUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
