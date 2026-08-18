using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Domain.Events;

public class InvoiceCreatedEvent : DomainEvent
    {
        public InvoiceCreatedEvent(Invoice item)
        {
            Item = item;
        }

        public Invoice Item { get; }
    }
public class InvoiceDeletedEvent : DomainEvent
{
    public InvoiceDeletedEvent(Invoice item)
    {
        Item = item;
    }

    public Invoice Item { get; }
}
public class InvoiceUpdatedEvent : DomainEvent
{
    public InvoiceUpdatedEvent(Invoice item)
    {
        Item = item;
    }

    public Invoice Item { get; }
}

