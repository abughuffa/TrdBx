namespace CleanArchitecture.Blazor.Application.Features.SPackages.Commands.Delete;

public class DeleteSPackageCommandValidator : AbstractValidator<DeleteSPackageCommand>
{
        public DeleteSPackageCommandValidator()
        {
          
            RuleFor(v => v.Id).NotNull().ForEach(v=>v.GreaterThan(0));
          
        }
}
    

