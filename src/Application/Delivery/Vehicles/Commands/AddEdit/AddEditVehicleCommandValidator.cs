
namespace CleanArchitecture.Blazor.Application.Features.Vehicles.Commands.AddEdit;

public class AddEditVehicleCommandValidator : AbstractValidator<AddEditVehicleCommand>
{
    public AddEditVehicleCommandValidator()
    {
                RuleFor(v => v.VehicleNo).MaximumLength(50).NotEmpty();
        RuleFor(v => v.VehicleTypeId).NotNull();

    }

}

