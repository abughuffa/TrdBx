namespace CleanArchitecture.Blazor.Application.Features.TrackingUnitModels.Commands.Delete;

public class DeleteTrackingUnitModelCommandValidator : AbstractValidator<DeleteTrackingUnitModelCommand>
{
        public DeleteTrackingUnitModelCommandValidator()
        {
          
            RuleFor(v => v.Id).NotNull().ForEach(v=>v.GreaterThan(0));
          
        }
}
    

