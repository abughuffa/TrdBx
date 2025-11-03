using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Domain.Events;

public class ActivateHostingTestCaseUpdatedEvent : DomainEvent
{
        public ActivateHostingTestCaseUpdatedEvent(ActivateHostingTestCase item)
        {
            Item = item;
        }

        public ActivateHostingTestCase Item { get; }
    }

public class ActivateHostingTestCaseDeletedEvent : DomainEvent
{
    public ActivateHostingTestCaseDeletedEvent(ActivateHostingTestCase item)
    {
        Item = item;
    }

    public ActivateHostingTestCase Item { get; }
}

