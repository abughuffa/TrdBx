namespace CleanArchitecture.Blazor.Application.Features.WialonTasks.Commands.Delete;

public class DeleteWialonTaskCommandValidator : AbstractValidator<DeleteWialonTaskCommand>
{
    public DeleteWialonTaskCommandValidator()
    {

        RuleFor(v => v.Id).NotNull().ForEach(v => v.GreaterThan(0));

    }
}


