namespace CleanArchitecture.Blazor.Application.Features.Tickets.Commands.Execute;

public class MarkAsExecutedTicketCommandValidator : AbstractValidator<MarkAsExecutedTicketCommand>
{
    public MarkAsExecutedTicketCommandValidator()
    {
        RuleFor(v => v.Id).NotNull();

    }

}

