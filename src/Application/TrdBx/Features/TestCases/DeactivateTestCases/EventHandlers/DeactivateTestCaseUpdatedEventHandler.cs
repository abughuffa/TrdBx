namespace CleanArchitecture.Blazor.Application.Features.TestCases.DeactivateTestCases.EventHandlers;

    public class DeactivateTestCaseUpdatedEventHandler : INotificationHandler<DeactivateTestCaseUpdatedEvent>
    {
        private readonly ILogger<DeactivateTestCaseUpdatedEventHandler> _logger;

        public DeactivateTestCaseUpdatedEventHandler(
            ILogger<DeactivateTestCaseUpdatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(DeactivateTestCaseUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
