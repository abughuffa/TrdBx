namespace CleanArchitecture.Blazor.Application.Features.Tickets.Commands.Accept;

public class AcceptTicketCommandValidator : AbstractValidator<AcceptTicketCommand>
{
    public AcceptTicketCommandValidator()
    {
        RuleFor(v => v.Id).NotNull();

    }

}

