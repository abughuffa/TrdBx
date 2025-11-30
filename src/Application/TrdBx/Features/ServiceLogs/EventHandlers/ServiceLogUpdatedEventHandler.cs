using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.ServiceLogs.EventHandlers;

    public class ServiceLogUpdatedEventHandler : INotificationHandler<ServiceLogUpdatedEvent>
    {
        private readonly ILogger<ServiceLogUpdatedEventHandler> _logger;

        public ServiceLogUpdatedEventHandler(
            ILogger<ServiceLogUpdatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(ServiceLogUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
