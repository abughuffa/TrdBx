using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.Tickets.EventHandlers;

    public class TicketUpdatedEventHandler : INotificationHandler<TicketUpdatedEvent>
    {
        private readonly ILogger<TicketUpdatedEventHandler> _logger;

        public TicketUpdatedEventHandler(
            ILogger<TicketUpdatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(TicketUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
