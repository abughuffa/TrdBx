namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.Delete;

public class DeleteTrackingUnitCommandValidator : AbstractValidator<DeleteTrackingUnitCommand>
{
        public DeleteTrackingUnitCommandValidator()
        {
          
            RuleFor(v => v.Id).NotNull().ForEach(v=>v.GreaterThan(0));
          
        }
}
    

