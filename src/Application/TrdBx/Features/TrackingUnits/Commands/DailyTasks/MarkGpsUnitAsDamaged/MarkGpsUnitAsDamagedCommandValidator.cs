namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.MarkTrackingUnitAsDamaged;

public class MarkTrackingUnitAsDamagedCommandValidator : AbstractValidator<MarkTrackingUnitAsDamagedCommand>
{
    public MarkTrackingUnitAsDamagedCommandValidator()
    {
        RuleFor(v => v.Id).NotNull();
        RuleFor(v => v.TsDate).NotNull().LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));

    }

}

