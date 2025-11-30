
namespace CleanArchitecture.Blazor.Application.Features.WialonUnits.Commands.Import;

public class ImportWialonUnitsCommandValidator : AbstractValidator<ImportWialonUnitsCommand>
{
    public ImportWialonUnitsCommandValidator()
    {

        RuleFor(v => v.Data)
             .NotNull()
             .NotEmpty();

    }
}

