using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.Subscriptions.EventHandlers;

public class SubscriptionCreatedEventHandler : INotificationHandler<SubscriptionCreatedEvent>
{
        private readonly ILogger<SubscriptionCreatedEventHandler> _logger;

        public SubscriptionCreatedEventHandler(
            ILogger<SubscriptionCreatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(SubscriptionCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
}
