namespace CleanArchitecture.Blazor.Application.Features.TrackingUnitModels.Commands.AddEdit;

public class AddEditTrackingUnitModelCommandValidator : AbstractValidator<AddEditTrackingUnitModelCommand>
{
    public AddEditTrackingUnitModelCommandValidator()
    {
                RuleFor(v => v.Name).MaximumLength(50).NotEmpty();
                RuleFor(v => v.WialonName).MaximumLength(50).NotEmpty();

    }

}

