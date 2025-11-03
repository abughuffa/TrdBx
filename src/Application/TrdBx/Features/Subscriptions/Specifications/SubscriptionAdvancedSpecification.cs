using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.Subscriptions.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for advanced filtering of Subscriptions.
/// </summary>
public class SubscriptionAdvancedSpecification : Specification<Subscription>
{
    public SubscriptionAdvancedSpecification(SubscriptionAdvancedFilter filter)
    {

        Query.Where(filter.Keyword, !string.IsNullOrEmpty(filter.Keyword))
             .Where(q => q.ServiceLogId == filter.ServiceLogId, !(filter.ServiceLogId.Equals(0) || filter.ServiceLogId.Equals(null)))
             .Where(q => q.TrackingUnitId == filter.TrackingUnitId, !(filter.TrackingUnitId.Equals(0) || filter.TrackingUnitId.Equals(null)));


    }
}
