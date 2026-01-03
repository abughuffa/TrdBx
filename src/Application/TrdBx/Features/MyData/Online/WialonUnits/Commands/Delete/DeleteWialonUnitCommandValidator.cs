namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.WialonUnits.Commands.Delete;

public class DeleteWialonUnitCommandValidator : AbstractValidator<DeleteWialonUnitCommand>
{
    public DeleteWialonUnitCommandValidator()
    {

        RuleFor(v => v.Id).NotNull().ForEach(v => v.GreaterThan(0));

    }
}


