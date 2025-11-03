using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Domain.Events;

public class ActivateTestCaseUpdatedEvent : DomainEvent
{
        public ActivateTestCaseUpdatedEvent(ActivateTestCase item)
        {
            Item = item;
        }

        public ActivateTestCase Item { get; }
    }
public class ActivateTestCaseDeletedEvent : DomainEvent
{
    public ActivateTestCaseDeletedEvent(ActivateTestCase item)
    {
        Item = item;
    }

    public ActivateTestCase Item { get; }
}

