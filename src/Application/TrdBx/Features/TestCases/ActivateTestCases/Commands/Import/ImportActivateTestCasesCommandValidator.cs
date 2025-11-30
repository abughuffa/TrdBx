namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateTestCases.Commands.Import;

public class ImportActivateTestCasesCommandValidator : AbstractValidator<ImportActivateTestCasesCommand>
{
    public ImportActivateTestCasesCommandValidator()
    {

        RuleFor(v => v.Data)
             .NotNull()
             .NotEmpty();
    }
}

