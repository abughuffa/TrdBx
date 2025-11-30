using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.Subscriptions.EventHandlers;

    public class SubscriptionDeletedEventHandler : INotificationHandler<SubscriptionDeletedEvent>
    {
        private readonly ILogger<SubscriptionDeletedEventHandler> _logger;

        public SubscriptionDeletedEventHandler(
            ILogger<SubscriptionDeletedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(SubscriptionDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
