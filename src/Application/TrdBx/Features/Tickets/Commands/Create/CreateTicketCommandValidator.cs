using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.Tickets.Commands.Create;

public class CreateTicketCommandValidator : AbstractValidator<CreateTicketCommand>
{
    

    public CreateTicketCommandValidator()
    {
        RuleFor(v => v.Id).NotNull().ForEach(v => v.GreaterThan(0));
        RuleFor(v => v.ServiceTask).NotNull();
        RuleFor(v => v.TcDate).NotNull().LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));
    }

}

