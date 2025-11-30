namespace CleanArchitecture.Blazor.Application.Features.Tickets.Commands.Start;

public class StartTicketCommandValidator : AbstractValidator<StartTicketCommand>
{
    public StartTicketCommandValidator()
    {
        RuleFor(v => v.Id).NotNull();

    }

}

