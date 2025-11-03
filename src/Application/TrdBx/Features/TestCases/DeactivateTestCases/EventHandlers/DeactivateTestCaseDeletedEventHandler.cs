using CleanArchitecture.Blazor.Domain.Events;

namespace CleanArchitecture.Blazor.Application.Features.TestCases.DeactivateTestCases.EventHandlers;

    public class DeactivateTestCaseDeletedEventHandler : INotificationHandler<DeactivateTestCaseDeletedEvent>
    {
        private readonly ILogger<DeactivateTestCaseDeletedEventHandler> _logger;

        public DeactivateTestCaseDeletedEventHandler(
            ILogger<DeactivateTestCaseDeletedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(DeactivateTestCaseDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
