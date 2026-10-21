namespace CleanArchitecture.Blazor.Application.Features.Invoices.Commands.AddPayment;

public class AddPaymentCommandValidator : AbstractValidator<AddPaymentCommand>
{
    public AddPaymentCommandValidator()
    {
        RuleFor(v => v.Id).NotNull();



    }

}

