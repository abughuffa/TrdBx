
namespace CleanArchitecture.Blazor.Application.Features.Shipments.Commands.AddEdit;

public class AddEditShipmentCommandValidator : AbstractValidator<AddEditShipmentCommand>
{
    public AddEditShipmentCommandValidator()
    {
                RuleFor(v => v.ShipmentNo).MaximumLength(50).NotEmpty(); 
    RuleFor(v => v.StartLocation).MaximumLength(255); 
    RuleFor(v => v.EndLocation).MaximumLength(255); 
    RuleFor(v => v.Price).NotNull(); 

     }

}

