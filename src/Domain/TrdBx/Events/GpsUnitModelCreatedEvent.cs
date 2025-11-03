using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Domain.Events;

public class TrackingUnitModelCreatedEvent : DomainEvent
    {
        public TrackingUnitModelCreatedEvent(TrackingUnitModel item)
        {
            Item = item;
        }

        public TrackingUnitModel Item { get; }
    }

public class TrackingUnitModelDeletedEvent : DomainEvent
{
    public TrackingUnitModelDeletedEvent(TrackingUnitModel item)
    {
        Item = item;
    }

    public TrackingUnitModel Item { get; }
}

public class TrackingUnitModelUpdatedEvent : DomainEvent
{
    public TrackingUnitModelUpdatedEvent(TrackingUnitModel item)
    {
        Item = item;
    }

    public TrackingUnitModel Item { get; }
}

