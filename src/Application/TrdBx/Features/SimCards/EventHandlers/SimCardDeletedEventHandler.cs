using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.SimCards.EventHandlers;

    public class SimCardDeletedEventHandler : INotificationHandler<SimCardDeletedEvent>
    {
        private readonly ILogger<SimCardDeletedEventHandler> _logger;

        public SimCardDeletedEventHandler(
            ILogger<SimCardDeletedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(SimCardDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
