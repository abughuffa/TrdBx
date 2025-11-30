namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateHostingTestCases.EventHandlers;

    public class ActivateHostingTestCaseUpdatedEventHandler : INotificationHandler<ActivateHostingTestCaseUpdatedEvent>
    {
        private readonly ILogger<ActivateHostingTestCaseUpdatedEventHandler> _logger;

        public ActivateHostingTestCaseUpdatedEventHandler(
            ILogger<ActivateHostingTestCaseUpdatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(ActivateHostingTestCaseUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
