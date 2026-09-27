
namespace CleanArchitecture.Blazor.Application.Features.SmsMessages.Commands.Resend;

public class ResendSmsCommandValidator : AbstractValidator<ResendSmsCommand>
{
    public ResendSmsCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.OverrideTo).MaximumLength(20).When(x => x.OverrideTo is not null);
        RuleFor(x => x.OverrideBody).MaximumLength(1600).When(x => x.OverrideBody is not null);
        RuleFor(x => x.OverrideSimSlot).InclusiveBetween(0, 3).When(x => x.OverrideSimSlot is not null);
    }
}