namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.MarkTrackingUnitAsLost;

public class MarkTrackingUnitAsLostCommandValidator : AbstractValidator<MarkTrackingUnitAsLostCommand>
{
    public MarkTrackingUnitAsLostCommandValidator()
    {
        RuleFor(v => v.Id).NotNull();
        RuleFor(v => v.TsDate).NotNull().LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));

    }

}

