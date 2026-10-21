namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.ImpulseCharts.Commands.Recharge;

public class RechargeSimCardCommandValidator : AbstractValidator<RechargeSimCardCommand>
{
    public RechargeSimCardCommandValidator()
    {
        RuleFor(v => v.Id).NotNull();
    }

}

