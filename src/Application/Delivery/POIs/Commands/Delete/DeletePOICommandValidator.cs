

namespace CleanArchitecture.Blazor.Application.Features.POIs.Commands.Delete;

public class DeletePOICommandValidator : AbstractValidator<DeletePOICommand>
{
        public DeletePOICommandValidator()
        {
          
            RuleFor(v => v.Id).NotNull().ForEach(v=>v.GreaterThan(0));
          
        }
}
    

