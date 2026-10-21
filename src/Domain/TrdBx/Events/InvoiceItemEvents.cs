namespace CleanArchitecture.Blazor.Domain.Events;

public class InvoiceItemCreatedEvent : DomainEvent
    {
        public InvoiceItemCreatedEvent(InvoiceItem item)
        {
            Item = item;
        }

        public InvoiceItem Item { get; }
    }
public class InvoiceItemDeletedEvent : DomainEvent
{
    public InvoiceItemDeletedEvent(InvoiceItem item)
    {
        Item = item;
    }

    public InvoiceItem Item { get; }
}
public class InvoiceItemUpdatedEvent : DomainEvent
{
    public InvoiceItemUpdatedEvent(InvoiceItem item)
    {
        Item = item;
    }

    public InvoiceItem Item { get; }
}

