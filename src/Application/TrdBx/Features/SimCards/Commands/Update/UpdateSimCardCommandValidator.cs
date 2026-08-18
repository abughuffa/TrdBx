namespace CleanArchitecture.Blazor.Application.Features.SimCards.Commands.Update;

public class UpdateSimCardCommandValidator : AbstractValidator<UpdateSimCardCommand>
{
    public UpdateSimCardCommandValidator()
    {
        RuleFor(v => v.Id).NotNull();
        RuleFor(v => v.SimCardNo).MaximumLength(50).NotEmpty();
        RuleFor(v => v.ICCID).MaximumLength(50).NotEmpty();
        RuleFor(v => v.SPackageId).NotNull();


    }

}

