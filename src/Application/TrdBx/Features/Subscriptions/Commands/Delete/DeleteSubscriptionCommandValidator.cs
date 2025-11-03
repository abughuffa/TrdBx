namespace CleanArchitecture.Blazor.Application.Features.Subscriptions.Commands.Delete;

public class DeleteSubscriptionCommandValidator : AbstractValidator<DeleteSubscriptionCommand>
{
        public DeleteSubscriptionCommandValidator()
        {
          
            RuleFor(v => v.Id).NotNull().ForEach(v=>v.GreaterThan(0));
          
        }
}
    

