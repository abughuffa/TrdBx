namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.WialonUnits.Commands.Import;

public class ImportWialonUnitsCommandValidator : AbstractValidator<ImportWialonUnitsCommand>
{
    public ImportWialonUnitsCommandValidator()
    {

        RuleFor(v => v.Data)
             .NotNull()
             .NotEmpty();

    }
}

