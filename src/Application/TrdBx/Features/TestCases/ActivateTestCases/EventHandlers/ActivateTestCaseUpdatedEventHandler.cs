namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateTestCases.EventHandlers;

    public class ActivateTestCaseUpdatedEventHandler : INotificationHandler<ActivateTestCaseUpdatedEvent>
    {
        private readonly ILogger<ActivateTestCaseUpdatedEventHandler> _logger;

        public ActivateTestCaseUpdatedEventHandler(
            ILogger<ActivateTestCaseUpdatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(ActivateTestCaseUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
