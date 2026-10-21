namespace CleanArchitecture.Blazor.Application.Features.SProviders.Commands.Import;

public class ImportSProvidersCommandValidator : AbstractValidator<ImportSProvidersCommand>
{
        public ImportSProvidersCommandValidator()
        {
           
           RuleFor(v => v.Data)
                .NotNull()
                .NotEmpty();

        }
}

