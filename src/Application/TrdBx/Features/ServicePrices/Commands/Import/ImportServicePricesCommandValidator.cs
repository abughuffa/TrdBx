namespace CleanArchitecture.Blazor.Application.Features.ServicePrices.Commands.Import;

public class ImportServicePricesCommandValidator : AbstractValidator<ImportServicePricesCommand>
{
        public ImportServicePricesCommandValidator()
        {
           
           RuleFor(v => v.Data)
                .NotNull()
                .NotEmpty();

        }
}

