namespace CleanArchitecture.Blazor.Application.Features.Tickets.Commands.Approve;

public class ApproveTicketCommandValidator : AbstractValidator<ApproveTicketCommand>
{
    public ApproveTicketCommandValidator()
    {
        RuleFor(v => v.Id).NotNull();

    }

}

