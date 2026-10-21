namespace CleanArchitecture.Blazor.Application.Features.SimCards.Commands.Delete;

public class DeleteSimCardCommandValidator : AbstractValidator<DeleteSimCardCommand>
{
    public DeleteSimCardCommandValidator()
    {

        RuleFor(v => v.Id).NotNull().ForEach(v => v.GreaterThan(0));

    }
}


