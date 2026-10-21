namespace CleanArchitecture.Blazor.Application.Features.TrackedAssets.Commands.Import;

public class ImportTrackedAssetsCommandValidator : AbstractValidator<ImportTrackedAssetsCommand>
{
    public ImportTrackedAssetsCommandValidator()
    {

        RuleFor(v => v.Data)
             .NotNull()
             .NotEmpty();

    }
}

