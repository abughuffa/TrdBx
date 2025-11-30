
namespace CleanArchitecture.Blazor.Application.Features.Warehouses.Commands.Create;

public class CreateWarehouseCommandValidator : AbstractValidator<CreateWarehouseCommand>
{
        public CreateWarehouseCommandValidator()
        {
        RuleFor(v => v.Name).MaximumLength(50).NotEmpty();
        RuleFor(v => v.Latitude).NotNull().NotEqual(0.0);
        RuleFor(v => v.Longitude).NotNull().NotEqual(0.0);

    }
       
}

