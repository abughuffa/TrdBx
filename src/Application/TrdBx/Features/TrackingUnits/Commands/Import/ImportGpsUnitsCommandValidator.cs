namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.Import;

public class ImportTrackingUnitsCommandValidator : AbstractValidator<ImportTrackingUnitsCommand>
{
        public ImportTrackingUnitsCommandValidator()
        {
           
           RuleFor(v => v.Data)
                .NotNull()
                .NotEmpty();

        }
}

