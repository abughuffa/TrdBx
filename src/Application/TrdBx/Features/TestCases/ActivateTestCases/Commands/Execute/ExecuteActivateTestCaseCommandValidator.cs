namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateTestCases.Commands.Execute;

public class ExecuteActivateTestCaseCommandValidator : AbstractValidator<ExecuteActivateTestCaseCommand>
{
    public ExecuteActivateTestCaseCommandValidator()
    {

        RuleFor(v => v.Id).NotNull().ForEach(v => v.GreaterThan(0));

    }
}


