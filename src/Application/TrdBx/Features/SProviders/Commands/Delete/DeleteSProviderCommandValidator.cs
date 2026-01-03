namespace CleanArchitecture.Blazor.Application.Features.SProviders.Commands.Delete;

public class DeleteSProviderCommandValidator : AbstractValidator<DeleteSProviderCommand>
{
        public DeleteSProviderCommandValidator()
        {
          
            RuleFor(v => v.Id).NotNull().ForEach(v=>v.GreaterThan(0));
          
        }
}
    

