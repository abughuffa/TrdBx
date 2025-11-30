

namespace CleanArchitecture.Blazor.Application.Features.VehicleTypes.Commands.Delete;

public class DeleteVehicleTypeCommandValidator : AbstractValidator<DeleteVehicleTypeCommand>
{
        public DeleteVehicleTypeCommandValidator()
        {
          
            RuleFor(v => v.Id).NotNull().ForEach(v=>v.GreaterThan(0));
          
        }
}
    

