namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateGprsTestCases.EventHandlers;

    public class ActivateGprsTestCaseDeletedEventHandler : INotificationHandler<ActivateGprsTestCaseDeletedEvent>
    {
        private readonly ILogger<ActivateGprsTestCaseDeletedEventHandler> _logger;

        public ActivateGprsTestCaseDeletedEventHandler(
            ILogger<ActivateGprsTestCaseDeletedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(ActivateGprsTestCaseDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
