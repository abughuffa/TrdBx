namespace CleanArchitecture.Blazor.Domain.Events;

public class LibyanaSimCardCreatedEvent : DomainEvent
    {
        public LibyanaSimCardCreatedEvent(LibyanaSimCard item)
        {
            Item = item;
        }
        public LibyanaSimCard Item { get; }
    }

public class LibyanaSimCardUpdatedEvent : DomainEvent
{
    public LibyanaSimCardUpdatedEvent(LibyanaSimCard item)
    {
        Item = item;
    }
    public LibyanaSimCard Item { get; }
}

public class LibyanaSimCardDeletedEvent : DomainEvent
{
    public LibyanaSimCardDeletedEvent(LibyanaSimCard item)
    {
        Item = item;
    }
    public LibyanaSimCard Item { get; }
}

