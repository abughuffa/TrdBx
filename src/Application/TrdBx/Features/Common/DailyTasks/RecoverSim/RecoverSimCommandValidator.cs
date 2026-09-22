namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.RecoverSim;

public class RecoverSimCommandValidator : AbstractValidator<RecoverSimCommand>
{
    public RecoverSimCommandValidator()
    {
        RuleFor(v => v.Id).NotNull();
    }

}

