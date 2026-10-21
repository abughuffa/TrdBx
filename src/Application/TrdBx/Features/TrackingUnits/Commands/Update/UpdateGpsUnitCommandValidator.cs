namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.Update;

public class UpdateTrackingUnitCommandValidator : AbstractValidator<UpdateTrackingUnitCommand>
{
        public UpdateTrackingUnitCommandValidator()
        {
           RuleFor(v => v.Id).NotNull();
        RuleFor(v => v.SNo).MaximumLength(50).NotEmpty();
        RuleFor(v => v.Imei).MaximumLength(255).NotEmpty();
        RuleFor(v => v.TrackingUnitModelId).NotNull();


    }
    
}

