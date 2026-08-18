namespace CleanArchitecture.Blazor.Application.Features.ServicePrices.Commands.Update;

public class UpdateServicePriceCommandValidator : AbstractValidator<UpdateServicePriceCommand>
{
        public UpdateServicePriceCommandValidator()
        {
           RuleFor(v => v.Id).NotNull();
    RuleFor(v => v.Description).MaximumLength(255).NotEmpty();

          
        }
    
}

