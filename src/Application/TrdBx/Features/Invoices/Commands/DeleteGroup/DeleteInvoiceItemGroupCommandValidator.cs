namespace CleanArchitecture.Blazor.Application.Features.Invoices.Commands.Delete;

public class DeleteInvoiceItemGroupCommandValidator : AbstractValidator<DeleteInvoiceItemGroupCommand>
{
    public DeleteInvoiceItemGroupCommandValidator()
    {

        RuleFor(v => v.Id).NotNull().GreaterThan(0);

    }
}


