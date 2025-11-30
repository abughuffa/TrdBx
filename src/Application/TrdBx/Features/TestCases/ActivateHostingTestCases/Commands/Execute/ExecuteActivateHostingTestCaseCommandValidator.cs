namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateHostingTestCases.Commands.Execute;

public class ExecuteActivateHostingTestCaseCommandValidator : AbstractValidator<ExecuteActivateHostingTestCaseCommand>
{
    public ExecuteActivateHostingTestCaseCommandValidator()
    {

        RuleFor(v => v.Id).NotNull().ForEach(v => v.GreaterThan(0));

    }
}


