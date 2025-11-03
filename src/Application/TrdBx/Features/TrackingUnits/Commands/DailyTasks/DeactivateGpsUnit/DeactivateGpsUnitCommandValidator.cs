namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.DeactivateTrackingUnit;

public class DeactivateTrackingUnitCommandValidator : AbstractValidator<DeactivateTrackingUnitCommand>
{
    public DeactivateTrackingUnitCommandValidator()
    {
        RuleFor(v => v.Id).NotNull();
        RuleFor(v => v.TsDate).NotNull().LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));

    }

}

