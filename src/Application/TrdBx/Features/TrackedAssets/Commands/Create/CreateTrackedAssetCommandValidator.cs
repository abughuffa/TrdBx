namespace CleanArchitecture.Blazor.Application.Features.TrackedAssets.Commands.Create;

public class CreateTrackedAssetCommandValidator : AbstractValidator<CreateTrackedAssetCommand>
{
    

    public CreateTrackedAssetCommandValidator()
    {
        RuleFor(v => v.TrackedAssetCode != null || v.PlateNo != null || v.VinSerNo != null).Equals(true);
        RuleFor(v => v.TrackedAssetDesc).MaximumLength(255).NotEmpty();

    }

}

