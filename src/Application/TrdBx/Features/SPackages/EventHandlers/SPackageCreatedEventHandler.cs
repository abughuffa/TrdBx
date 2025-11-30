using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.SPackages.EventHandlers;

public class SPackageCreatedEventHandler : INotificationHandler<SPackageCreatedEvent>
{
        private readonly ILogger<SPackageCreatedEventHandler> _logger;

        public SPackageCreatedEventHandler(
            ILogger<SPackageCreatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(SPackageCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
}
