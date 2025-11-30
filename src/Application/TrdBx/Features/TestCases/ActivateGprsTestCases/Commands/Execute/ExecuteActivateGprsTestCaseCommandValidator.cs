namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateGprsTestCases.Commands.Execute;

public class ExecuteActivateGprsTestCaseCommandValidator : AbstractValidator<ExecuteActivateGprsTestCaseCommand>
{
    public ExecuteActivateGprsTestCaseCommandValidator()
    {

        RuleFor(v => v.Id).NotNull().ForEach(v => v.GreaterThan(0));

    }
}


