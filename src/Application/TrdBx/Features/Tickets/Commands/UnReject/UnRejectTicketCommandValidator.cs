namespace CleanArchitecture.Blazor.Application.Features.Tickets.Commands.UnReject;

public class UnRejectTicketCommandValidator : AbstractValidator<UnRejectTicketCommand>
{
    public UnRejectTicketCommandValidator()
    {
        RuleFor(v => v.Id).NotNull();

    }

}

