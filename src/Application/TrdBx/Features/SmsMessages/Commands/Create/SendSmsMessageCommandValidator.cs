namespace CleanArchitecture.Blazor.Application.Features.SmsMessages.Commands.Send;

public class SendSmsMessageCommandValidator : AbstractValidator<SendSmsMessageCommand>
{

    public SendSmsMessageCommandValidator()
    {
        RuleFor(v => v.PhoneNumber).MaximumLength(20).NotEmpty();
        RuleFor(v => v.Message).MaximumLength(200).NotEmpty();
    }

}

