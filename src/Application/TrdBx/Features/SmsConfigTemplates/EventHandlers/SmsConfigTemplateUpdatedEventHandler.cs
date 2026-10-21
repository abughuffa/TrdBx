using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.SmsConfigTemplates.EventHandlers;

    public class SmsConfigTemplateUpdatedEventHandler : INotificationHandler<SmsConfigTemplateUpdatedEvent>
    {
        private readonly ILogger<SmsConfigTemplateUpdatedEventHandler> _logger;

        public SmsConfigTemplateUpdatedEventHandler(
            ILogger<SmsConfigTemplateUpdatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public ValueTask Handle(SmsConfigTemplateUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return ValueTask.CompletedTask;
        }
    }
