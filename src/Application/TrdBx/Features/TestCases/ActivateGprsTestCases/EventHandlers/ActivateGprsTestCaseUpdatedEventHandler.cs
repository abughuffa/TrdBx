namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateGprsTestCases.EventHandlers;

    public class ActivateGprsTestCaseUpdatedEventHandler : INotificationHandler<ActivateGprsTestCaseUpdatedEvent>
    {
        private readonly ILogger<ActivateGprsTestCaseUpdatedEventHandler> _logger;

        public ActivateGprsTestCaseUpdatedEventHandler(
            ILogger<ActivateGprsTestCaseUpdatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(ActivateGprsTestCaseUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
