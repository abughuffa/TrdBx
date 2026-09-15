namespace CleanArchitecture.Blazor.Application.Features.SmsMessages.Commands.CreateOutgoing;

public class CreateOutgoingSmsMessageCommandValidator : AbstractValidator<CreateOutgoingSmsMessageCommand>
{

    public CreateOutgoingSmsMessageCommandValidator()
    {
        RuleFor(v => v.PhoneNumber).MaximumLength(20).NotEmpty();
        RuleFor(v => v.Message).MaximumLength(200).NotEmpty();
    }

}

