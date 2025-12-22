
namespace CleanArchitecture.Blazor.Application.Features.Shipments.Commands.Create;

public class CreateShipmentCommandValidator : AbstractValidator<CreateShipmentCommand>
{
        public CreateShipmentCommandValidator()
        {
        RuleFor(v => v.ShipmentNo).MaximumLength(50).NotEmpty();
        RuleFor(v => v.WayPoints.Count()).GreaterThan(1);
        RuleFor(v => v.Price).NotNull();

    }
       
}

