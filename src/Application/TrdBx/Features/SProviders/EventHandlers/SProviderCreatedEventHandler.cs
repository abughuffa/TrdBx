using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.SProviders.EventHandlers;

public class SProviderCreatedEventHandler : INotificationHandler<SProviderCreatedEvent>
{
        private readonly ILogger<SProviderCreatedEventHandler> _logger;

        public SProviderCreatedEventHandler(
            ILogger<SProviderCreatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(SProviderCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
}
