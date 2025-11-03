namespace CleanArchitecture.Blazor.Application.Features.Subscriptions.Commands.Import;

public class ImportSubscriptionsCommandValidator : AbstractValidator<ImportSubscriptionsCommand>
{
        public ImportSubscriptionsCommandValidator()
        {
           
           RuleFor(v => v.Data)
                .NotNull()
                .NotEmpty();

        }
}

