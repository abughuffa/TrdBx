namespace CleanArchitecture.Blazor.Application.Features.SmsMessages.Commands.Delete;

public class DeleteSmsMessageCommandValidator : AbstractValidator<DeleteSmsMessageCommand>
{
    public DeleteSmsMessageCommandValidator()
    {

        RuleFor(v => v.Id).NotNull().ForEach(v => v.GreaterThan(0));

    }
}


