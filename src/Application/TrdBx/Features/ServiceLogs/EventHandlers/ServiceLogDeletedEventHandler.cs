using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.ServiceLogs.EventHandlers;

    public class ServiceLogDeletedEventHandler : INotificationHandler<ServiceLogDeletedEvent>
    {
        private readonly ILogger<ServiceLogDeletedEventHandler> _logger;

        public ServiceLogDeletedEventHandler(
            ILogger<ServiceLogDeletedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(ServiceLogDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
