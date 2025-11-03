namespace CleanArchitecture.Blazor.Application.Features.Invoices.Commands.Create;

public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
{
    public CreateInvoiceCommandValidator()
    {
        RuleFor(v => v.InvDate).NotNull();
        RuleFor(v => v.CustomerId).NotNull();


    }

}

