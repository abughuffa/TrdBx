namespace CleanArchitecture.Blazor.Application.Features.SProviders.Commands.AddEdit;

public class AddEditSProviderCommandValidator : AbstractValidator<AddEditSProviderCommand>
{
    public AddEditSProviderCommandValidator()
    {
                RuleFor(v => v.Name).MaximumLength(50).NotEmpty(); 


     }

}

