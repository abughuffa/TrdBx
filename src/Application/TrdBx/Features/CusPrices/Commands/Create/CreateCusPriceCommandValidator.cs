namespace CleanArchitecture.Blazor.Application.Features.CusPrices.Commands.Create;

public class CreateCusPriceCommandValidator : AbstractValidator<CreateCusPriceCommand>
{
    public CreateCusPriceCommandValidator()
    {
        RuleFor(v => v.CustomerId).NotNull();
        RuleFor(v => v.TrackingUnitModelId).NotNull();
        RuleFor(v => v.Host).GreaterThan(0.0m);
        RuleFor(v => v.Gprs).GreaterThan(0.0m);
        RuleFor(v => v.Price).GreaterThan(0.0m);


    }

}

