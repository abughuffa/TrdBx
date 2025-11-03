using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.Tickets.EventHandlers;

    public class TicketDeletedEventHandler : INotificationHandler<TicketDeletedEvent>
    {
        private readonly ILogger<TicketDeletedEventHandler> _logger;

        public TicketDeletedEventHandler(
            ILogger<TicketDeletedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(TicketDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
