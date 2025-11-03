namespace CleanArchitecture.Blazor.Application.Features.Subscriptions.Commands.AddEdit;

public class AddEditSubscriptionCommandValidator : AbstractValidator<AddEditSubscriptionCommand>
{
    public AddEditSubscriptionCommandValidator()
    {
        RuleFor(v => v.TrackingUnitId).NotNull();
        RuleFor(v => v.SubPackageFees).NotNull();
        RuleFor(v => v.SsDate).NotNull();
        RuleFor(v => v.SeDate).NotNull();
        RuleFor(v => v.LastPaidFees).NotNull();
        RuleFor(v => v.Desc).MaximumLength(255).NotEmpty();



    }

}

