namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateGprsTestCases.Commands.Delete;

public class DeleteActivateGprsTestCaseCommandValidator : AbstractValidator<DeleteActivateGprsTestCaseCommand>
{
        public DeleteActivateGprsTestCaseCommandValidator()
        {
          
            RuleFor(v => v.Id).NotNull().ForEach(v=>v.GreaterThan(0));
          
        }
}
    

