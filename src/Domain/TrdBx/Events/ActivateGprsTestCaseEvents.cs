using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Domain.Events;

public class ActivateGprsTestCaseUpdatedEvent : DomainEvent
{
        public ActivateGprsTestCaseUpdatedEvent(ActivateGprsTestCase item)
        {
            Item = item;
        }

        public ActivateGprsTestCase Item { get; }
    }

public class ActivateGprsTestCaseDeletedEvent : DomainEvent
{
    public ActivateGprsTestCaseDeletedEvent(ActivateGprsTestCase item)
    {
        Item = item;
    }

    public ActivateGprsTestCase Item { get; }
}
