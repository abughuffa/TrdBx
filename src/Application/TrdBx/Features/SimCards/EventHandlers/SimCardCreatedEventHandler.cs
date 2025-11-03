using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.SimCards.EventHandlers;

public class SimCardCreatedEventHandler : INotificationHandler<SimCardCreatedEvent>
{
        private readonly ILogger<SimCardCreatedEventHandler> _logger;

        public SimCardCreatedEventHandler(
            ILogger<SimCardCreatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(SimCardCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
}
