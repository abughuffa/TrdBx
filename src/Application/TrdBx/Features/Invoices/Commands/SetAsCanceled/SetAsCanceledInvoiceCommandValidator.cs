namespace CleanArchitecture.Blazor.Application.Features.Invoices.Commands.SetAsCanceled;

public class SetAsCanceledInvoiceCommandValidator : AbstractValidator<SetAsCanceledInvoiceCommand>
{
    public SetAsCanceledInvoiceCommandValidator()
    {
        RuleFor(v => v.Id).NotNull().GreaterThan(0);
    }
}

