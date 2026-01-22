using CleanArchitecture.Blazor.Application.Features.Invoices.Commands.Create;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.SimCards.Commands.Create;

public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
{

    public CreateInvoiceCommandValidator()
    {
        RuleFor(v => v.CustomerId).NotNull();
        RuleFor(v => v.InvoiceType).NotEqual(InvoiceType.All);
        RuleFor(v => v.InvoiceDate).NotNull().LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));
        RuleFor(v => v.DiscountRate).NotNull().InclusiveBetween(0.0m,100.0m);
        RuleFor(v => v.TaxRate).NotNull().InclusiveBetween(0.0m, 100.0m);
    }

}

