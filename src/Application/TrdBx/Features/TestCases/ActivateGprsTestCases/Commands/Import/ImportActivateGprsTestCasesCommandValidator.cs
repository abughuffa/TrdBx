namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateGprsTestCases.Commands.Import;

public class ImportActivateGprsTestCasesCommandValidator : AbstractValidator<ImportActivateGprsTestCasesCommand>
{
    public ImportActivateGprsTestCasesCommandValidator()
    {

        RuleFor(v => v.Data)
             .NotNull()
             .NotEmpty();
    }
}

