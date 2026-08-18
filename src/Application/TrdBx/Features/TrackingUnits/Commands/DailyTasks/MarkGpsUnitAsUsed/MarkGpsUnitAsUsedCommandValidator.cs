namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.MarkTrackingUnitAsUsed;

public class MarkTrackingUnitAsUsedCommandValidator : AbstractValidator<MarkTrackingUnitAsUsedCommand>
{
    public MarkTrackingUnitAsUsedCommandValidator()
    {
        RuleFor(v => v.Id).NotNull();
        RuleFor(v => v.TsDate).NotNull().LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));

    }

}

