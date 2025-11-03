using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.TrackedAssets.EventHandlers;

public class TrackedAssetCreatedEventHandler : INotificationHandler<TrackedAssetCreatedEvent>
{
        private readonly ILogger<TrackedAssetCreatedEventHandler> _logger;

        public TrackedAssetCreatedEventHandler(
            ILogger<TrackedAssetCreatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(TrackedAssetCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
}
