using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Domain.Events;

public class TrackedAssetCreatedEvent : DomainEvent
    {
        public TrackedAssetCreatedEvent(TrackedAsset item)
        {
            Item = item;
        }

        public TrackedAsset Item { get; }
    }
public class TrackedAssetDeletedEvent : DomainEvent
{
    public TrackedAssetDeletedEvent(TrackedAsset item)
    {
        Item = item;
    }

    public TrackedAsset Item { get; }
}
public class TrackedAssetUpdatedEvent : DomainEvent
{
    public TrackedAssetUpdatedEvent(TrackedAsset item)
    {
        Item = item;
    }

    public TrackedAsset Item { get; }
}

