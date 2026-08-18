namespace CleanArchitecture.Blazor.Application.Features.Invoices.Commands.SetNextStatus;

public class SetNextStatusInvoiceCommandValidator : AbstractValidator<SetNextStatusInvoiceCommand>
{
    public SetNextStatusInvoiceCommandValidator()
    {
        RuleFor(v => v.Id).NotNull().GreaterThan(0);
    }
}

