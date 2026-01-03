namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.LibyanaSimCards.Commands.Import;

public class ImportLibyanaSimCardsCommandValidator : AbstractValidator<ImportLibyanaSimCardsCommand>
{
    public ImportLibyanaSimCardsCommandValidator()
    {

        RuleFor(v => v.Data)
             .NotNull()
             .NotEmpty();

    }
}

