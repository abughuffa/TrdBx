using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Domain.Events;

public class WialonTaskCreatedEvent : DomainEvent
    {
        public WialonTaskCreatedEvent(WialonTask item)
        {
            Item = item;
        }

        public WialonTask Item { get; }
    }
public class WialonTaskUpdatedEvent : DomainEvent
{
    public WialonTaskUpdatedEvent(WialonTask item)
    {
        Item = item;
    }

    public WialonTask Item { get; }
}
public class WialonTaskDeletedEvent : DomainEvent
{
    public WialonTaskDeletedEvent(WialonTask item)
    {
        Item = item;
    }

    public WialonTask Item { get; }
}


