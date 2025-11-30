namespace CleanArchitecture.Blazor.Application.Features.ServiceLogs.Commands.Delete;

public class DeleteServiceLogCommandValidator : AbstractValidator<DeleteServiceLogCommand>
{
        public DeleteServiceLogCommandValidator()
        {
          
            RuleFor(v => v.Id).NotNull().ForEach(v=>v.GreaterThan(0));
          
        }
}
    

