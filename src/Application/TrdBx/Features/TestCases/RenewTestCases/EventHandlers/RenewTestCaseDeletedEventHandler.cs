
//namespace CleanArchitecture.Blazor.Application.Features.RenewTestCases.EventHandlers;

//    public class RenewTestCaseDeletedEventHandler : INotificationHandler<RenewTestCaseDeletedEvent>
//    {
//        private readonly ILogger<RenewTestCaseDeletedEventHandler> _logger;

//        public RenewTestCaseDeletedEventHandler(
//            ILogger<RenewTestCaseDeletedEventHandler> logger
//            )
//        {
//            _logger = logger;
//        }
//        public Task Handle(RenewTestCaseDeletedEvent notification, CancellationToken cancellationToken)
//        {
//            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
//            return Task.CompletedTask;
//        }
//    }
