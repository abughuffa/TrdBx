namespace CleanArchitecture.Blazor.Application.Features.Invoices.Commands.SetAsPaid;

public class SetAsPaidInvoiceCommandValidator : AbstractValidator<SetAsPaidInvoiceCommand>
{
    public SetAsPaidInvoiceCommandValidator()
    {
        RuleFor(v => v.Id).NotNull().GreaterThan(0);
    }
}

