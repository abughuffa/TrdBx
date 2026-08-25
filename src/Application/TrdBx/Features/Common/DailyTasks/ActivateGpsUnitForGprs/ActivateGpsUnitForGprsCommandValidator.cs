namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.ActivateTrackingUnitForGprs;

public class ActivateTrackingUnitForGprsCommandValidator : AbstractValidator<ActivateTrackingUnitForGprsCommand>
{
    public ActivateTrackingUnitForGprsCommandValidator()
    {
        RuleFor(v => v.Id).NotNull();
        RuleFor(v => v.TsDate).NotNull().LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));

    }

}

