namespace CleanArchitecture.Blazor.Application.Features.WialonTasks.Commands.Execute;

public class ExecuteWialonTaskCommandValidator : AbstractValidator<ExecuteWialonTaskCommand>
{
    public ExecuteWialonTaskCommandValidator()
    {

        RuleFor(v => v.Id).NotNull().GreaterThan(0);
        RuleFor(v => v.ExcDate).NotNull().LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));

    }
}


