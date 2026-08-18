namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.WialonUnits.Commands.Update;

public class UpdateWialonUnitCommandValidator : AbstractValidator<UpdateWialonUnitCommand>
{
    public UpdateWialonUnitCommandValidator()
    {
        RuleFor(v => v.Id).NotNull();
        RuleFor(v => v.StatusOnWialon).Must(x => x.Equals("Active") || x.Equals("Inactive"));
        RuleFor(v => v.UnitSNo).NotNull().NotEmpty();
        RuleFor(v => v.SimCardNo).NotNull().NotEmpty();

    }

}

