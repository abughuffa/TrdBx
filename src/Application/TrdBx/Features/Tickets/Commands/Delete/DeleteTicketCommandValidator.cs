namespace CleanArchitecture.Blazor.Application.Features.Tickets.Commands.Delete;

public class DeleteTicketCommandValidator : AbstractValidator<DeleteTicketCommand>
{
        public DeleteTicketCommandValidator()
        {
          
            RuleFor(v => v.Id).NotNull().ForEach(v=>v.GreaterThan(0));
          
        }
}
    

