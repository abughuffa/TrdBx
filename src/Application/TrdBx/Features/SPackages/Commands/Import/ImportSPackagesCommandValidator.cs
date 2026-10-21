namespace CleanArchitecture.Blazor.Application.Features.SPackages.Commands.Import;

public class ImportSPackagesCommandValidator : AbstractValidator<ImportSPackagesCommand>
{
        public ImportSPackagesCommandValidator()
        {
           
           RuleFor(v => v.Data)
                .NotNull()
                .NotEmpty();

        }
}

