namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.Charts.Commands.Recharge;

public class RechargeSimCardCommandValidator : AbstractValidator<RechargeSimCardCommand>
{
    public RechargeSimCardCommandValidator()
    {
        RuleFor(v => v.Id).NotNull();
    }

}

