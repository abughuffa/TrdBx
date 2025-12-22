

namespace CleanArchitecture.Blazor.Application.Features.Shipments.Commands.Update;

public class UpdateShipmentCommandValidator : AbstractValidator<UpdateShipmentCommand>
{
        public UpdateShipmentCommandValidator()
        {
        RuleFor(v => v.ShipmentNo).MaximumLength(50).NotEmpty();
        RuleFor(v => v.WayPoints.Count()).GreaterThan(1);
        RuleFor(v => v.Price).NotNull();


    }
    
}

