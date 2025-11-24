

namespace CleanArchitecture.Blazor.Application.Features.Warehouses.Commands.Delete;

public class DeleteWarehouseCommandValidator : AbstractValidator<DeleteWarehouseCommand>
{
        public DeleteWarehouseCommandValidator()
        {
          
            RuleFor(v => v.Id).NotNull().ForEach(v=>v.GreaterThan(0));
          
        }
}
    

