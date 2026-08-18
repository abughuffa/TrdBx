using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Domain.Events;

public class TicketCreatedEvent : DomainEvent
{
    public TicketCreatedEvent(Ticket item)
    {
        Item = item;
    }

    public Ticket Item { get; }
}
public class TicketDeletedEvent : DomainEvent
{
    public TicketDeletedEvent(Ticket item)
    {
        Item = item;
    }

    public Ticket Item { get; }
}

public class TicketUpdatedEvent : DomainEvent
{
    public TicketUpdatedEvent(Ticket item)
    {
        Item = item;
    }

    public Ticket Item { get; }
}