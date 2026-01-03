namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.LibyanaSimCards.Commands.Delete;

public class DeleteLibyanaSimCardCommandValidator : AbstractValidator<DeleteLibyanaSimCardCommand>
{
    public DeleteLibyanaSimCardCommandValidator()
    {

        RuleFor(v => v.Id).NotNull().ForEach(v => v.GreaterThan(0));

    }
}


