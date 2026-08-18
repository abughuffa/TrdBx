using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Domain.Events;

public class SimCardCreatedEvent : DomainEvent
    {
        public SimCardCreatedEvent(SimCard item)
        {
            Item = item;
        }

        public SimCard Item { get; }
    }

public class SimCardDeletedEvent : DomainEvent
{
    public SimCardDeletedEvent(SimCard item)
    {
        Item = item;
    }

    public SimCard Item { get; }
}

public class SimCardUpdatedEvent : DomainEvent
{
    public SimCardUpdatedEvent(SimCard item)
    {
        Item = item;
    }

    public SimCard Item { get; }
}
