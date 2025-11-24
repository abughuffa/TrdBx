

namespace CleanArchitecture.Blazor.Application.Features.Vehicles.Commands.Update;

public class UpdateVehicleCommandValidator : AbstractValidator<UpdateVehicleCommand>
{
        public UpdateVehicleCommandValidator()
        {
        RuleFor(v => v.VehicleNo).MaximumLength(50).NotEmpty();
        RuleFor(v => v.VehicleTypeId).NotNull();


    }
    
}

