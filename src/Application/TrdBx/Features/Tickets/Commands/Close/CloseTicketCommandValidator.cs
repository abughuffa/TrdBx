namespace CleanArchitecture.Blazor.Application.Features.Tickets.Commands.Close;

public class CloseTicketCommandValidator : AbstractValidator<CloseTicketCommand>
{
    public CloseTicketCommandValidator()
    {
        RuleFor(v => v.Id).NotNull();
        RuleFor(v => v.TeDate).NotNull().LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));

    }

}

