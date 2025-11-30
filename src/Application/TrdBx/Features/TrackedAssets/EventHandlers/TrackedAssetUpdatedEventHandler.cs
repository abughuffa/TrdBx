using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.TrackedAssets.EventHandlers;

    public class TrackedAssetUpdatedEventHandler : INotificationHandler<TrackedAssetUpdatedEvent>
    {
        private readonly ILogger<TrackedAssetUpdatedEventHandler> _logger;

        public TrackedAssetUpdatedEventHandler(
            ILogger<TrackedAssetUpdatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(TrackedAssetUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
