using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.ServicePrices.EventHandlers;

    public class ServicePriceUpdatedEventHandler : INotificationHandler<ServicePriceUpdatedEvent>
    {
        private readonly ILogger<ServicePriceUpdatedEventHandler> _logger;

        public ServicePriceUpdatedEventHandler(
            ILogger<ServicePriceUpdatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(ServicePriceUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
