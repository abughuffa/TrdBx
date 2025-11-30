namespace CleanArchitecture.Blazor.Application.Features.SimCards.Commands.AddEdit;

public class AddEditSimCardCommandValidator : AbstractValidator<AddEditSimCardCommand>
{
    public AddEditSimCardCommandValidator()
    {
        RuleFor(v => v.Id).NotNull();
        RuleFor(v => v.SimCardNo).MaximumLength(50).NotEmpty();
        RuleFor(v => v.ICCID).MaximumLength(50).NotEmpty();
        RuleFor(v => v.SPackageId).NotNull();

    }

}

