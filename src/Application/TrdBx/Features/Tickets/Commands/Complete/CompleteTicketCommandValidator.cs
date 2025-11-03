namespace CleanArchitecture.Blazor.Application.Features.Tickets.Commands.Complete;

public class CompleteTicketCommandValidator : AbstractValidator<CompleteTicketCommand>
{
    public CompleteTicketCommandValidator()
    {
        RuleFor(v => v.Id).NotNull();
        RuleFor(v => v.TeDate).NotNull().LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));

    }

}

