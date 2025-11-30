namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.Transfer;

public class TransferTrackingUnitCommandValidator : AbstractValidator<TransferTrackingUnitCommand>
{
    public TransferTrackingUnitCommandValidator()
    {
        RuleFor(v => v.Id).NotNull();
        RuleFor(v => v.SimCardId).NotNull();
        RuleFor(v => v.TrackedAssetId).NotNull();
        RuleFor(v => v.CustomerId).NotNull();
        RuleFor(v => v.InstallerId).NotNull();
        RuleFor(v => v.SubPackage).NotNull();
        RuleFor(v => v.InsMode).NotNull();
        RuleFor(v => v.TsDate).NotNull().LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));
        RuleFor(v => v.CreateDeservedServices).NotNull();
    }

}

