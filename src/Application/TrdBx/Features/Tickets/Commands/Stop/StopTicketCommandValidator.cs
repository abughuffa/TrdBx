namespace CleanArchitecture.Blazor.Application.Features.Tickets.Commands.Stop;

public class StopTicketCommandValidator : AbstractValidator<StopTicketCommand>
{
    public StopTicketCommandValidator()
    {
        RuleFor(v => v.Id).NotNull();

    }

}

