namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.ActivateTrackingUnitForHosting;

public class ActivateTrackingUnitForHostingCommandValidator : AbstractValidator<ActivateTrackingUnitForHostingCommand>
{
    public ActivateTrackingUnitForHostingCommandValidator()
    {
        RuleFor(v => v.Id).NotNull();
        RuleFor(v => v.TsDate).NotNull().LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));

    }

}

