namespace CleanArchitecture.Blazor.Application.Features.TrackedAssets.Commands.Delete;

public class DeleteTrackedAssetCommandValidator : AbstractValidator<DeleteTrackedAssetCommand>
{
        public DeleteTrackedAssetCommandValidator()
        {
          
            RuleFor(v => v.Id).NotNull().ForEach(v=>v.GreaterThan(0));
          
        }
}
    

