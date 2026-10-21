namespace CleanArchitecture.Blazor.Application.Features.TrackingUnitModels.Commands.Import;

public class ImportTrackingUnitModelsCommandValidator : AbstractValidator<ImportTrackingUnitModelsCommand>
{
        public ImportTrackingUnitModelsCommandValidator()
        {
           
           RuleFor(v => v.Data)
                .NotNull()
                .NotEmpty();

        }
}

