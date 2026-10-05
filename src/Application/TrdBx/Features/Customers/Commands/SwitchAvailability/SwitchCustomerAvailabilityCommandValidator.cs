using CleanArchitecture.Blazor.Application.Features.Customers.Commands.Update;

namespace CleanArchitecture.Blazor.Application.Features.Customers.Commands.SwitchAvailability;

public class SwitchCustomerAvailabilityCommandValidator : AbstractValidator<SwitchCustomerAvailabilityCommand>
{
    public SwitchCustomerAvailabilityCommandValidator()
    {
        RuleFor(v => v.Id).NotNull();


    }

}

