namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.ReassignOwner;

public class ReassignTrackingUnitOwnerCommandValidator : AbstractValidator<ReassignTrackingUnitOwnerCommand>
{
    public ReassignTrackingUnitOwnerCommandValidator()
    {
        RuleFor(v => v.Id).NotNull();
        RuleFor(v => v.CustomerId).NotNull();
    }

}

