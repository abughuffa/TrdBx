namespace CleanArchitecture.Blazor.Application.Features.CusPrices.Commands.Delete;

public class DeleteCusPriceCommandValidator : AbstractValidator<DeleteCusPriceCommand>
{
    public DeleteCusPriceCommandValidator()
    {

        RuleFor(v => v.Id).NotNull().ForEach(v => v.GreaterThan(0));

    }
}


