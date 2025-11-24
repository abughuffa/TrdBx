

namespace CleanArchitecture.Blazor.Application.Features.Vehicles.Commands.Delete;

public class DeleteVehicleCommandValidator : AbstractValidator<DeleteVehicleCommand>
{
        public DeleteVehicleCommandValidator()
        {
          
            RuleFor(v => v.Id).NotNull().ForEach(v=>v.GreaterThan(0));
          
        }
}
    

