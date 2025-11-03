using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Domain.Events;

public class CustomerCreatedEvent : DomainEvent
    {
        public CustomerCreatedEvent(Customer item)
        {
            Item = item;
        }

        public Customer Item { get; }
    }

public class CustomerDeletedEvent : DomainEvent
{
    public CustomerDeletedEvent(Customer item)
    {
        Item = item;
    }

    public Customer Item { get; }
}


public class CustomerUpdatedEvent : DomainEvent
{
    public CustomerUpdatedEvent(Customer item)
    {
        Item = item;
    }

    public Customer Item { get; }
}

