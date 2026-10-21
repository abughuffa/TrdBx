using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Domain.Events;

public class TrackingUnitCreatedEvent : DomainEvent
    {
        public TrackingUnitCreatedEvent(TrackingUnit item)
        {
            Item = item;
        }

        public TrackingUnit Item { get; }
    }

public class TrackingUnitDeletedEvent : DomainEvent
{
    public TrackingUnitDeletedEvent(TrackingUnit item)
    {
        Item = item;
    }

    public TrackingUnit Item { get; }
}

public class TrackingUnitUpdatedEvent : DomainEvent
{
    public TrackingUnitUpdatedEvent(TrackingUnit item)
    {
        Item = item;
    }

    public TrackingUnit Item { get; }
}

