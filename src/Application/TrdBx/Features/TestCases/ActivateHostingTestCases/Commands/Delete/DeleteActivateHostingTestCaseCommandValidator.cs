namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateHostingTestCases.Commands.Delete;

public class DeleteActivateHostingTestCaseCommandValidator : AbstractValidator<DeleteActivateHostingTestCaseCommand>
{
    public DeleteActivateHostingTestCaseCommandValidator()
    {

        RuleFor(v => v.Id).NotNull().ForEach(v => v.GreaterThan(0));

    }
}


