namespace CleanArchitecture.Blazor.Application.Features.Invoices.Commands.Delete;

public class DeleteInvoiceCommandValidator : AbstractValidator<DeleteInvoiceCommand>
{
    public DeleteInvoiceCommandValidator()
    {

        RuleFor(v => v.Id).NotNull().GreaterThan(0);

    }
}


