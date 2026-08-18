namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.Reserve;

public class ReserveTrackingUnitCommandValidator : AbstractValidator<ReserveTrackingUnitCommand>
{
    public ReserveTrackingUnitCommandValidator()
    {
        RuleFor(v => v.Id).NotNull();
        RuleFor(v => v.CustomerId).NotNull();
    }

}

