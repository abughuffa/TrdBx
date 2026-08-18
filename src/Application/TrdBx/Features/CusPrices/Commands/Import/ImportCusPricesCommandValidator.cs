namespace CleanArchitecture.Blazor.Application.Features.CusPrices.Commands.Import;

public class ImportCusPricesCommandValidator : AbstractValidator<ImportCusPricesCommand>
{
    public ImportCusPricesCommandValidator()
    {

        RuleFor(v => v.Data)
             .NotNull()
             .NotEmpty();

    }
}

