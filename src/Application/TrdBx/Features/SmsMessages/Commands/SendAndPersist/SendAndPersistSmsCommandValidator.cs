 namespace CleanArchitecture.Blazor.Application.Features.SmsMessages.Commands.SendAndPersist;

public class SendAndPersistSmsCommandValidator : AbstractValidator<SendAndPersistSmsCommand>
{
    public SendAndPersistSmsCommandValidator()
    {
        RuleFor(x => x.To).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Body).NotEmpty().MaximumLength(1600);
        RuleFor(x => x.SimSlot).InclusiveBetween(0, 1);
    }
}

