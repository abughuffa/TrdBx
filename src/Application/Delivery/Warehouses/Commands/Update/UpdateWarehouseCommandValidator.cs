

namespace CleanArchitecture.Blazor.Application.Features.Warehouses.Commands.Update;

public class UpdateWarehouseCommandValidator : AbstractValidator<UpdateWarehouseCommand>
{
        public UpdateWarehouseCommandValidator()
        {
        RuleFor(v => v.Name).MaximumLength(50).NotEmpty();
        RuleFor(v => v.Latitude).NotNull().NotEqual(0.0);
        RuleFor(v => v.Longitude).NotNull().NotEqual(0.0);


    }
    
}

