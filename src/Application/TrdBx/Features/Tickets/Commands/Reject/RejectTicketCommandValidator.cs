namespace CleanArchitecture.Blazor.Application.Features.Tickets.Commands.Reject;

public class RejectTicketCommandValidator : AbstractValidator<RejectTicketCommand>
{
    public RejectTicketCommandValidator()
    {
        RuleFor(v => v.Id).NotNull();
        RuleFor(v => v.Note).NotNull();

    }

}

