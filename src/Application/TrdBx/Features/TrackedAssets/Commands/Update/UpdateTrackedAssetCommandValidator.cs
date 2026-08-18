namespace CleanArchitecture.Blazor.Application.Features.TrackedAssets.Commands.Update;

public class UpdateTrackedAssetCommandValidator : AbstractValidator<UpdateTrackedAssetCommand>
{
        public UpdateTrackedAssetCommandValidator()
        {
        RuleFor(v => v.TrackedAssetCode != null || v.PlateNo != null || v.VinSerNo != null).Equals(true);
        RuleFor(v => v.TrackedAssetDesc).MaximumLength(255).NotEmpty();


    }
    
}

