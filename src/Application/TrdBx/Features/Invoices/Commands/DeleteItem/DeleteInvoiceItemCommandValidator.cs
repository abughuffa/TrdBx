namespace CleanArchitecture.Blazor.Application.Features.Invoices.Commands.Delete;

public class DeleteInvoiceItemCommandValidator : AbstractValidator<DeleteInvoiceItemCommand>
{
    public DeleteInvoiceItemCommandValidator()
    {

        RuleFor(v => v.Id).NotNull().GreaterThan(0);

    }
}


