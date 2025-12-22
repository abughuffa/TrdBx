
namespace CleanArchitecture.Blazor.Application.Features.POIs.Commands.Create;

public class CreatePOICommandValidator : AbstractValidator<CreatePOICommand>
{
        public CreatePOICommandValidator()
        {
        RuleFor(v => v.Name).MaximumLength(50).NotEmpty();
        RuleFor(v => v.Latitude).NotNull().NotEqual(0.0);
        RuleFor(v => v.Longitude).NotNull().NotEqual(0.0);

    }
       
}

