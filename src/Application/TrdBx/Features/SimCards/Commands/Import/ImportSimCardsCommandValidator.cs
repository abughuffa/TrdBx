namespace CleanArchitecture.Blazor.Application.Features.SimCards.Commands.Import;

public class ImportSimCardsCommandValidator : AbstractValidator<ImportSimCardsCommand>
{
    public ImportSimCardsCommandValidator()
    {

        RuleFor(v => v.Data)
             .NotNull()
             .NotEmpty();

    }
}

