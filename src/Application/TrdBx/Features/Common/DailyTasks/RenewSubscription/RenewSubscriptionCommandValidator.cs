// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT licen

namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Commands.DailyTasks.RenewSubscription;

public class RenewSubscriptionCommandValidator : AbstractValidator<RenewSubscriptionCommand>
{
    public RenewSubscriptionCommandValidator()
    {
        RuleFor(v => v.Id).NotNull();
        RuleFor(v => v.TsDate).NotNull().GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));

    }

}

