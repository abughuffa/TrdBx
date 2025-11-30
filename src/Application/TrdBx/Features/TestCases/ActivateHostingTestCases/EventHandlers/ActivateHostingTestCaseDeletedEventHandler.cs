namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateHostingTestCases.EventHandlers;

    public class ActivateHostingTestCaseDeletedEventHandler : INotificationHandler<ActivateHostingTestCaseDeletedEvent>
    {
        private readonly ILogger<ActivateHostingTestCaseDeletedEventHandler> _logger;

        public ActivateHostingTestCaseDeletedEventHandler(
            ILogger<ActivateHostingTestCaseDeletedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(ActivateHostingTestCaseDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
