namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateHostingTestCases.Commands.Import;

public class ImportActivateHostingTestCasesCommandValidator : AbstractValidator<ImportActivateHostingTestCasesCommand>
{
    public ImportActivateHostingTestCasesCommandValidator()
    {

        RuleFor(v => v.Data)
             .NotNull()
             .NotEmpty();
    }
}

