namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.Create;

public class CreateTrackingUnitCommandValidator : AbstractValidator<CreateTrackingUnitCommand>
{
        public CreateTrackingUnitCommandValidator()
        {
        RuleFor(v => v.SNo).MaximumLength(50).NotEmpty();
        RuleFor(v => v.Imei).MaximumLength(255).NotEmpty();
        RuleFor(v => v.TrackingUnitModelId).NotNull();

    }
       
}

