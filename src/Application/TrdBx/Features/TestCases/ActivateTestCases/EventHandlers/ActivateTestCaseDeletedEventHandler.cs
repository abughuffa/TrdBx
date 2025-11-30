namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateTestCases.EventHandlers;

    public class ActivateTestCaseDeletedEventHandler : INotificationHandler<ActivateTestCaseDeletedEvent>
    {
        private readonly ILogger<ActivateTestCaseDeletedEventHandler> _logger;

        public ActivateTestCaseDeletedEventHandler(
            ILogger<ActivateTestCaseDeletedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(ActivateTestCaseDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
