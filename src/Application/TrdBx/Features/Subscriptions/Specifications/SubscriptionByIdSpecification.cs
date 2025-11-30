using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.Subscriptions.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for filtering Subscriptions by their ID.
/// </summary>
public class SubscriptionByIdSpecification : Specification<Subscription>
{
    public SubscriptionByIdSpecification(int id)
    {
        Query.Where(q => q.Id == id);
    }
}