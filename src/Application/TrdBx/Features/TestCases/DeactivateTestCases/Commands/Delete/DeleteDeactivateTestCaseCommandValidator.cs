namespace CleanArchitecture.Blazor.Application.Features.TestCases.DeactivateTestCases.Commands.Delete;

public class DeleteDeactivateTestCaseCommandValidator : AbstractValidator<DeleteDeactivateTestCaseCommand>
{
        public DeleteDeactivateTestCaseCommandValidator()
        {
          
            RuleFor(v => v.Id).NotNull().ForEach(v=>v.GreaterThan(0));
          
        }
}
    

