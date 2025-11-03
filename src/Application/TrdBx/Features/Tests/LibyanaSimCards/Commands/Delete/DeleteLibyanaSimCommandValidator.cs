namespace CleanArchitecture.Blazor.Application.Features.LibyanaSimCards.Commands.Delete;

public class DeleteLibyanaSimCardCommandValidator : AbstractValidator<DeleteLibyanaSimCardCommand>
{
    public DeleteLibyanaSimCardCommandValidator()
    {

        RuleFor(v => v.Id).NotNull().ForEach(v => v.GreaterThan(0));

    }
}


