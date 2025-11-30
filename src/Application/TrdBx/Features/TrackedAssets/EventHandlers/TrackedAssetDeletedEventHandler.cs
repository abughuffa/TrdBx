using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.TrackedAssets.EventHandlers;

    public class TrackedAssetDeletedEventHandler : INotificationHandler<TrackedAssetDeletedEvent>
    {
        private readonly ILogger<TrackedAssetDeletedEventHandler> _logger;

        public TrackedAssetDeletedEventHandler(
            ILogger<TrackedAssetDeletedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(TrackedAssetDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
