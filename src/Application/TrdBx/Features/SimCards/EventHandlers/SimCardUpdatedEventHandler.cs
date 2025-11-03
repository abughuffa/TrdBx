using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.SimCards.EventHandlers;

    public class SimCardUpdatedEventHandler : INotificationHandler<SimCardUpdatedEvent>
    {
        private readonly ILogger<SimCardUpdatedEventHandler> _logger;

        public SimCardUpdatedEventHandler(
            ILogger<SimCardUpdatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(SimCardUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
