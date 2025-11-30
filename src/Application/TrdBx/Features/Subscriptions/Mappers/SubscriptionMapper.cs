
using CleanArchitecture.Blazor.Application.Features.SimCards.DTOs;
using CleanArchitecture.Blazor.Application.Features.Subscriptions.Commands.AddEdit;
//using CleanArchitecture.Blazor.Application.Features.Subscriptions.Commands.Create;
//using CleanArchitecture.Blazor.Application.Features.Subscriptions.Commands.Update;
using CleanArchitecture.Blazor.Application.Features.Subscriptions.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.Subscriptions.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class Mapper
{
    public static partial SubscriptionDto ToDto(Subscription source);

    [MapperIgnoreSource(nameof(Subscription.ServiceLog))]
    [MapperIgnoreSource(nameof(Subscription.TrackingUnit))]
    public static partial Subscription FromDto(SubscriptionDto dto);
    public static partial Subscription FromEditCommand(AddEditSubscriptionCommand command);
    //public static partial Subscription FromCreateCommand(CreateSubscriptionCommand command);
    //public static partial UpdateSubscriptionCommand ToUpdateCommand(SubscriptionDto dto);
    public static partial AddEditSubscriptionCommand CloneFromDto(SubscriptionDto dto);
    //public static partial void ApplyChangesFrom(UpdateSubscriptionCommand source, Subscription target);
    public static partial void ApplyChangesFrom(AddEditSubscriptionCommand source, Subscription target);
    public static partial IQueryable<SubscriptionDto> ProjectTo(this IQueryable<Subscription> q);
}

