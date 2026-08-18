namespace CleanArchitecture.Blazor.Application.Features.SimCards.Commands.SetUsed;

public class SetUsedSimCardCommandValidator : AbstractValidator<SetUsedSimCardCommand>
{
    public SetUsedSimCardCommandValidator()
    {
        RuleFor(v => v.Id).NotNull();


    }

}

