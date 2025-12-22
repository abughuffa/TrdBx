

namespace CleanArchitecture.Blazor.Application.Features.POIs.Commands.Update;

public class UpdatePOICommandValidator : AbstractValidator<UpdatePOICommand>
{
        public UpdatePOICommandValidator()
        {
        RuleFor(v => v.Name).MaximumLength(50).NotEmpty();
        RuleFor(v => v.Latitude).NotNull().NotEqual(0.0);
        RuleFor(v => v.Longitude).NotNull().NotEqual(0.0);


    }
    
}

