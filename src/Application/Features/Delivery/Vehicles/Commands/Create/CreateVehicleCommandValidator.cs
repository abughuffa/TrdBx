
namespace CleanArchitecture.Blazor.Application.Features.Vehicles.Commands.Create;

public class CreateVehicleCommandValidator : AbstractValidator<CreateVehicleCommand>
{
        public CreateVehicleCommandValidator()
        {
        RuleFor(v => v.VehicleNo).MaximumLength(50).NotEmpty();
        RuleFor(v => v.VehicleTypeId).NotNull();

    }
       
}

