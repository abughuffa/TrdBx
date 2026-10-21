namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.InstallOrReplaceSim;

public class InstallOrReplaceSimCommandValidator : AbstractValidator<InstallOrReplaceSimCommand>
{
    public InstallOrReplaceSimCommandValidator()
    {
        RuleFor(v => v.Id).NotNull();
        RuleFor(v => v.SimCardId).NotNull();
    }

}

