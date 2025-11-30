using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Domain.Events;



public class ServicePriceUpdatedEvent : DomainEvent
{
    public ServicePriceUpdatedEvent(ServicePrice item)
    {
        Item = item;
    }

    public ServicePrice Item { get; }
}
