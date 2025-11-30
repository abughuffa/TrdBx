namespace CleanArchitecture.Blazor.Application.Features.TestCases.DeactivateTestCases.Commands.Execute;

public class ExecuteDeactivateTestCaseCommandValidator : AbstractValidator<ExecuteDeactivateTestCaseCommand>
{
    public ExecuteDeactivateTestCaseCommandValidator()
    {

        RuleFor(v => v.Id).NotNull().ForEach(v => v.GreaterThan(0));

    }
}


