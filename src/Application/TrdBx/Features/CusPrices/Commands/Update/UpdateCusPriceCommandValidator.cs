namespace CleanArchitecture.Blazor.Application.Features.CusPrices.Commands.Update;

public class UpdateCusPriceCommandValidator : AbstractValidator<UpdateCusPriceCommand>
{
    public UpdateCusPriceCommandValidator()
    {
        RuleFor(v => v.Id).NotNull().GreaterThan(0);
        RuleFor(v => v.Gprs).NotNull();
        RuleFor(v => v.Host).NotNull();
        RuleFor(v => v.Price).NotNull();
    }

}

