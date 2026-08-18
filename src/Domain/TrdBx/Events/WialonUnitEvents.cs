namespace CleanArchitecture.Blazor.Domain.Events;

public class WialonUnitCreatedEvent : DomainEvent
    {
        public WialonUnitCreatedEvent(WialonUnit item)
        {
            Item = item;
        }
        public WialonUnit Item { get; }
    }

public class WialonUnitUpdatedEvent : DomainEvent
{
    public WialonUnitUpdatedEvent(WialonUnit item)
    {
        Item = item;
    }
    public WialonUnit Item { get; }
}

public class WialonUnitDeletedEvent : DomainEvent
{
    public WialonUnitDeletedEvent(WialonUnit item)
    {
        Item = item;
    }
    public WialonUnit Item { get; }
}

