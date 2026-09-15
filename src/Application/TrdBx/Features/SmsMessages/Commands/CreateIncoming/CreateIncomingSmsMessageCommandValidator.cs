namespace CleanArchitecture.Blazor.Application.Features.SmsMessages.Commands.CreateIncoming;

public class CreateIncomingSmsMessageCommandValidator : AbstractValidator<CreateIncomingSmsMessageCommand>
{

    public CreateIncomingSmsMessageCommandValidator()
    {
        RuleFor(v => v.PhoneNumber).MaximumLength(20).NotEmpty();
        RuleFor(v => v.Message).MaximumLength(200).NotEmpty();
    }

}

