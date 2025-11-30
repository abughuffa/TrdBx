
namespace CleanArchitecture.Blazor.Application.Features.ActivateTestCases.Commands.Delete;

public class DeleteActivateTestCaseCommandValidator : AbstractValidator<DeleteActivateTestCaseCommand>
{
        public DeleteActivateTestCaseCommandValidator()
        {
          
            RuleFor(v => v.Id).NotNull().ForEach(v=>v.GreaterThan(0));
          
        }
}
    

