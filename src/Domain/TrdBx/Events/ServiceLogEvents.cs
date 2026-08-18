using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Domain.Events;

public class ServiceLogCreatedEvent : DomainEvent
{
    public ServiceLogCreatedEvent(ServiceLog item)
    {
        Item = item;
    }

    public ServiceLog Item { get; }
}
public class ServiceLogUpdatedEvent : DomainEvent
{
    public ServiceLogUpdatedEvent(ServiceLog item)
    {
        Item = item;
    }

    public ServiceLog Item { get; }
}
public class ServiceLogDeletedEvent : DomainEvent
{
    public ServiceLogDeletedEvent(ServiceLog item)
    {
        Item = item;
    }

    public ServiceLog Item { get; }
}