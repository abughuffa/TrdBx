using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Domain.Events;

public class DeactivateTestCaseUpdatedEvent : DomainEvent
{
        public DeactivateTestCaseUpdatedEvent(DeactivateTestCase item)
        {
            Item = item;
        }

        public DeactivateTestCase Item { get; }
    }
public class DeactivateTestCaseDeletedEvent : DomainEvent
{
    public DeactivateTestCaseDeletedEvent(DeactivateTestCase item)
    {
        Item = item;
    }

    public DeactivateTestCase Item { get; }
}
