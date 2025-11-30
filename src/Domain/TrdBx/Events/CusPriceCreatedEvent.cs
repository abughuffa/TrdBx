using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Domain.Events;

public class CusPriceCreatedEvent : DomainEvent
    {
        public CusPriceCreatedEvent(CusPrice item)
        {
            Item = item;
        }

        public CusPrice Item { get; }
    }

public class CusPriceDeletedEvent : DomainEvent
{
    public CusPriceDeletedEvent(CusPrice item)
    {
        Item = item;
    }

    public CusPrice Item { get; }
}

public class CusPriceUpdatedEvent : DomainEvent
{
    public CusPriceUpdatedEvent(CusPrice item)
    {
        Item = item;
    }

    public CusPrice Item { get; }
}

