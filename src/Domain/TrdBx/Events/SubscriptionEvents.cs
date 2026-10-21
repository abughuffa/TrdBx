using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Domain.Events;

public class SubscriptionCreatedEvent : DomainEvent
    {
        public SubscriptionCreatedEvent(Subscription item)
        {
            Item = item;
        }

        public Subscription Item { get; }
    }
public class SubscriptionUpdatedEvent : DomainEvent
{
    public SubscriptionUpdatedEvent(Subscription item)
    {
        Item = item;
    }

    public Subscription Item { get; }
}
public class SubscriptionDeletedEvent : DomainEvent
{
    public SubscriptionDeletedEvent(Subscription item)
    {
        Item = item;
    }

    public Subscription Item { get; }
}


