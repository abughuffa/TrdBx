namespace CleanArchitecture.Blazor.Application.Features.LibyanaSimCards.Commands.Import;

public class ImportLibyanaSimCardsCommandValidator : AbstractValidator<ImportLibyanaSimCardsCommand>
{
    public ImportLibyanaSimCardsCommandValidator()
    {

        RuleFor(v => v.Data)
             .NotNull()
             .NotEmpty();

    }
}

