using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Domain.Events;

public class InvoiceItemGroupCreatedEvent : DomainEvent
    {
        public InvoiceItemGroupCreatedEvent(InvoiceItemGroup item)
        {
            Item = item;
        }

        public InvoiceItemGroup Item { get; }
    }
public class InvoiceItemGroupDeletedEvent : DomainEvent
{
    public InvoiceItemGroupDeletedEvent(InvoiceItemGroup item)
    {
        Item = item;
    }

    public InvoiceItemGroup Item { get; }
}
public class InvoiceItemGroupUpdatedEvent : DomainEvent
{
    public InvoiceItemGroupUpdatedEvent(InvoiceItemGroup item)
    {
        Item = item;
    }

    public InvoiceItemGroup Item { get; }
}

