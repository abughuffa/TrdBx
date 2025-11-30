namespace CleanArchitecture.Blazor.Application.Features.SimCards.Commands.Create;

public class CreateSimCardCommandValidator : AbstractValidator<CreateSimCardCommand>
{

    public CreateSimCardCommandValidator()
    {
        RuleFor(v => v.SimCardNo).MaximumLength(50).NotEmpty();
        RuleFor(v => v.ICCID).MaximumLength(50).NotEmpty();
        RuleFor(v => v.SPackageId).NotNull();

    }

}

