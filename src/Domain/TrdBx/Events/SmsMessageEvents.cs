using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Domain.Events;

public class SmsMessageCreatedEvent : DomainEvent
    {
        public SmsMessageCreatedEvent(SmsMessage item)
        {
            Item = item;
        }

        public SmsMessage Item { get; }
    }

public class SmsMessageDeletedEvent : DomainEvent
{
    public SmsMessageDeletedEvent(SmsMessage item)
    {
        Item = item;
    }

    public SmsMessage Item { get; }
}

public class SmsMessageUpdatedEvent : DomainEvent
{
    public SmsMessageUpdatedEvent(SmsMessage item)
    {
        Item = item;
    }

    public SmsMessage Item { get; }
}
