using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.SPackages.EventHandlers;

    public class SPackageUpdatedEventHandler : INotificationHandler<SPackageUpdatedEvent>
    {
        private readonly ILogger<SPackageUpdatedEventHandler> _logger;

        public SPackageUpdatedEventHandler(
            ILogger<SPackageUpdatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(SPackageUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
