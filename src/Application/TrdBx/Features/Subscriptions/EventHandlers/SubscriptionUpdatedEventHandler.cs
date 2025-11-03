using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.Subscriptions.EventHandlers;

    public class SubscriptionUpdatedEventHandler : INotificationHandler<SubscriptionUpdatedEvent>
    {
        private readonly ILogger<SubscriptionUpdatedEventHandler> _logger;

        public SubscriptionUpdatedEventHandler(
            ILogger<SubscriptionUpdatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(SubscriptionUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
