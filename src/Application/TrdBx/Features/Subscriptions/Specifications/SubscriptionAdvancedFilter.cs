namespace CleanArchitecture.Blazor.Application.Features.Subscriptions.Specifications;


/// <summary>
/// A class for applying advanced filtering options to Subscription lists.
/// </summary>
public class SubscriptionAdvancedFilter: PaginationFilter
{
    public int? TrackingUnitId { get; set; }
    public int? ServiceLogId { get; set; }

}