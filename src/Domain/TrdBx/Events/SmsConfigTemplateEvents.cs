using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Domain.Events;

public class SmsConfigTemplateCreatedEvent : DomainEvent
    {
        public SmsConfigTemplateCreatedEvent(SmsConfigTemplate item)
        {
            Item = item;
        }

        public SmsConfigTemplate Item { get; }
    }

public class SmsConfigTemplateDeletedEvent : DomainEvent
{
    public SmsConfigTemplateDeletedEvent(SmsConfigTemplate item)
    {
        Item = item;
    }

    public SmsConfigTemplate Item { get; }
}

public class SmsConfigTemplateUpdatedEvent : DomainEvent
{
    public SmsConfigTemplateUpdatedEvent(SmsConfigTemplate item)
    {
        Item = item;
    }

    public SmsConfigTemplate Item { get; }
}

