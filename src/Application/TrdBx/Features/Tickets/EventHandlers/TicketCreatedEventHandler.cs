using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.Tickets.EventHandlers;

public class TicketCreatedEventHandler : INotificationHandler<TicketCreatedEvent>
{
        private readonly ILogger<TicketCreatedEventHandler> _logger;

        public TicketCreatedEventHandler(
            ILogger<TicketCreatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(TicketCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
}
