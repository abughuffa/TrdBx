namespace CleanArchitecture.Blazor.Application.Features.Customers.Commands.Import;

public class ImportCustomersCommandValidator : AbstractValidator<ImportCustomersCommand>
{
    public ImportCustomersCommandValidator()
    {

        RuleFor(v => v.Data)
             .NotNull()
             .NotEmpty();

    }
}

