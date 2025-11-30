// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT licen

namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.Renew;

public class RenewTrackingUnitSubscriptionCommandValidator : AbstractValidator<RenewTrackingUnitSubscriptionCommand>
{
    public RenewTrackingUnitSubscriptionCommandValidator()
    {
        RuleFor(v => v.Id).NotNull();
        RuleFor(v => v.TsDate).NotNull().GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));

    }

}

