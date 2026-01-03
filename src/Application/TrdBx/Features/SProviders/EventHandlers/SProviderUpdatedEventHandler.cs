using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.SProviders.EventHandlers;

    public class SProviderUpdatedEventHandler : INotificationHandler<SProviderUpdatedEvent>
    {
        private readonly ILogger<SProviderUpdatedEventHandler> _logger;

        public SProviderUpdatedEventHandler(
            ILogger<SProviderUpdatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(SProviderUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
