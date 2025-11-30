using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.ServiceLogs.EventHandlers;

public class ServiceLogCreatedEventHandler : INotificationHandler<ServiceLogCreatedEvent>
{
        private readonly ILogger<ServiceLogCreatedEventHandler> _logger;

        public ServiceLogCreatedEventHandler(
            ILogger<ServiceLogCreatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(ServiceLogCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
}
