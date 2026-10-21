using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Domain.Events;

public class SProviderCreatedEvent : DomainEvent
    {
        public SProviderCreatedEvent(SProvider item)
        {
            Item = item;
        }

        public SProvider Item { get; }
    }

public class SProviderDeletedEvent : DomainEvent
{
    public SProviderDeletedEvent(SProvider item)
    {
        Item = item;
    }

    public SProvider Item { get; }
}

public class SProviderUpdatedEvent : DomainEvent
{
    public SProviderUpdatedEvent(SProvider item)
    {
        Item = item;
    }

    public SProvider Item { get; }
}

