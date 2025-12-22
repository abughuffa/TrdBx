
using DocumentFormat.OpenXml.Wordprocessing;

namespace CleanArchitecture.Blazor.Application.Features.POIs.Commands.AddEdit;

public class AddEditPOICommandValidator : AbstractValidator<AddEditPOICommand>
{
    public AddEditPOICommandValidator()
    {
                RuleFor(v => v.Name).MaximumLength(50).NotEmpty(); 
    RuleFor(v => v.Latitude).NotNull().NotEqual(0.0);
        RuleFor(v => v.Longitude).NotNull().NotEqual(0.0);

    }

}

