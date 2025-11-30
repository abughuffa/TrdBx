
namespace CleanArchitecture.Blazor.Application.Features.VehicleTypes.Commands.AddEdit;

public class AddEditVehicleTypeCommandValidator : AbstractValidator<AddEditVehicleTypeCommand>
{
    public AddEditVehicleTypeCommandValidator()
    {
        RuleFor(v => v.Name).MaximumLength(50).NotEmpty();
    }

}

