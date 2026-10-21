namespace CleanArchitecture.Blazor.Application.Features.Invoices.Commands.SetAsCanceled;

public class SetAsCanceledXInvoiceCommandValidator : AbstractValidator<SetAsCanceledInvoiceCommand>
{
    public SetAsCanceledXInvoiceCommandValidator()
    {
        RuleFor(v => v.Id).NotNull().GreaterThan(0);
    }
}

