
//namespace CleanArchitecture.Blazor.Application.Features.RenewTestCases.EventHandlers;

//    public class RenewTestCaseUpdatedEventHandler : INotificationHandler<RenewTestCaseUpdatedEvent>
//    {
//        private readonly ILogger<RenewTestCaseUpdatedEventHandler> _logger;

//        public RenewTestCaseUpdatedEventHandler(
//            ILogger<RenewTestCaseUpdatedEventHandler> logger
//            )
//        {
//            _logger = logger;
//        }
//        public Task Handle(RenewTestCaseUpdatedEvent notification, CancellationToken cancellationToken)
//        {
//            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
//            return Task.CompletedTask;
//        }
//    }
