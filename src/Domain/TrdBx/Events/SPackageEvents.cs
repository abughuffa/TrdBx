using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Domain.Events;

public class SPackageCreatedEvent : DomainEvent
    {
        public SPackageCreatedEvent(SPackage item)
        {
            Item = item;
        }

        public SPackage Item { get; }
    }

public class SPackageDeletedEvent : DomainEvent
{
    public SPackageDeletedEvent(SPackage item)
    {
        Item = item;
    }

    public SPackage Item { get; }
}

public class SPackageUpdatedEvent : DomainEvent
{
    public SPackageUpdatedEvent(SPackage item)
    {
        Item = item;
    }

    public SPackage Item { get; }
}

