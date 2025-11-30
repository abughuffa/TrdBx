namespace CleanArchitecture.Blazor.Application.Features.Invoices.Commands.SetPreviousStatus;

public class SetPreviousStatusInvoiceCommandValidator : AbstractValidator<SetPreviousStatusInvoiceCommand>
{
    public SetPreviousStatusInvoiceCommandValidator()
    {
        RuleFor(v => v.Id).NotNull().GreaterThan(0);
    }
}

