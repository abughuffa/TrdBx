using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.SPackages.EventHandlers;

    public class SPackageDeletedEventHandler : INotificationHandler<SPackageDeletedEvent>
    {
        private readonly ILogger<SPackageDeletedEventHandler> _logger;

        public SPackageDeletedEventHandler(
            ILogger<SPackageDeletedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(SPackageDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
