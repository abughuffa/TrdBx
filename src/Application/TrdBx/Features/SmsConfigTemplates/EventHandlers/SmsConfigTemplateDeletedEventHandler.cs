using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.SmsConfigTemplates.EventHandlers;

    public class SmsConfigTemplateDeletedEventHandler : INotificationHandler<SmsConfigTemplateDeletedEvent>
    {
        private readonly ILogger<SmsConfigTemplateDeletedEventHandler> _logger;

        public SmsConfigTemplateDeletedEventHandler(
            ILogger<SmsConfigTemplateDeletedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public ValueTask Handle(SmsConfigTemplateDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return ValueTask.CompletedTask;
        }
    }
