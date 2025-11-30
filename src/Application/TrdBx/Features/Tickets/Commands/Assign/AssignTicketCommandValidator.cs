namespace CleanArchitecture.Blazor.Application.Features.Tickets.Commands.Assign;

public class AssignTicketCommandValidator : AbstractValidator<AssignTicketCommand>
{
    public AssignTicketCommandValidator()
    {
        RuleFor(v => v.Id).NotNull();
        RuleFor(v => v.TaDate).NotNull().LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));

    }

}

