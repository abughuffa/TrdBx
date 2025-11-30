namespace CleanArchitecture.Blazor.Application.Features.TestCases.DeactivateTestCases.Commands.Import;
public class ImportDeactivateTestCasesCommandValidator : AbstractValidator<ImportDeactivateTestCasesCommand>
{
    public ImportDeactivateTestCasesCommandValidator()
    {

        RuleFor(v => v.Data)
             .NotNull()
             .NotEmpty();
    }
}

