using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.SmsConfigTemplates.EventHandlers;

public class SmsConfigTemplateCreatedEventHandler : INotificationHandler<SmsConfigTemplateCreatedEvent>
{
        private readonly ILogger<SmsConfigTemplateCreatedEventHandler> _logger;

        public SmsConfigTemplateCreatedEventHandler(
            ILogger<SmsConfigTemplateCreatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public ValueTask Handle(SmsConfigTemplateCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return ValueTask.CompletedTask;
        }
}
