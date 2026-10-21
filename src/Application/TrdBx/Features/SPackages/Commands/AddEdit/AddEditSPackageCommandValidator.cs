namespace CleanArchitecture.Blazor.Application.Features.SPackages.Commands.AddEdit;

public class AddEditSPackageCommandValidator : AbstractValidator<AddEditSPackageCommand>
{
    public AddEditSPackageCommandValidator()
    {
        RuleFor(v => v.SProviderId).NotNull();
        RuleFor(v => v.Name).MaximumLength(50).NotEmpty(); 


     }

}

