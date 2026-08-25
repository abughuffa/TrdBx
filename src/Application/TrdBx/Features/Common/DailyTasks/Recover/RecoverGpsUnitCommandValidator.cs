namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.Recover;

public class RecoverTrackingUnitCommandValidator : AbstractValidator<RecoverTrackingUnitCommand>
{
    public RecoverTrackingUnitCommandValidator()
    {
        RuleFor(v => v.Id).NotNull();
        RuleFor(v => v.TsDate).NotNull().LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));
        RuleFor(v => v.CreateDeservedServices).NotNull();
    }

}

