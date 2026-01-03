using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.SProviders.EventHandlers;

    public class SProviderDeletedEventHandler : INotificationHandler<SProviderDeletedEvent>
    {
        private readonly ILogger<SProviderDeletedEventHandler> _logger;

        public SProviderDeletedEventHandler(
            ILogger<SProviderDeletedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(SProviderDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
