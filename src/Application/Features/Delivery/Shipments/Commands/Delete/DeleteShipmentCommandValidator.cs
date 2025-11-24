

namespace CleanArchitecture.Blazor.Application.Features.Shipments.Commands.Delete;

public class DeleteShipmentCommandValidator : AbstractValidator<DeleteShipmentCommand>
{
        public DeleteShipmentCommandValidator()
        {
          
            RuleFor(v => v.Id).NotNull().ForEach(v=>v.GreaterThan(0));
          
        }
}
    

