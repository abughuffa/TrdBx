namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.Check;

public class CheckTrackingUnitCommandValidator : AbstractValidator<CheckTrackingUnitCommand>
{
    public CheckTrackingUnitCommandValidator()
    {
        RuleFor(v => v.Id).NotNull();
        RuleFor(v => v.TsDate).NotNull().LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));

    }

}

