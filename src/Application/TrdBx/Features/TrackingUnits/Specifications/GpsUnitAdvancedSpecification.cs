using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for advanced filtering of TrackingUnits.
/// </summary>
public class TrackingUnitAdvancedSpecification : Specification<TrackingUnit>
{
    public TrackingUnitAdvancedSpecification(TrackingUnitAdvancedFilter filter)
    {
        var today = DateTime.UtcNow;
        var todayrange = today.GetDateRange(TrackingUnitListView.TODAY.ToString(), filter.LocalTimezoneOffset);
        var last30daysrange = today.GetDateRange(TrackingUnitListView.LAST_30_DAYS.ToString(),filter.LocalTimezoneOffset);

        Query.Where(q => q.SNo != null)
             .Where(filter.Keyword,!string.IsNullOrEmpty(filter.Keyword))
             //.Where(q => q.CreatedBy == filter.CurrentUser.UserId, filter.ListView == TrackingUnitListView.My && filter.CurrentUser is not null)
             .Where(x => x.Created >= todayrange.Start && x.Created < todayrange.End.AddDays(1), filter.ListView == TrackingUnitListView.TODAY)
             .Where(x => x.Created >= last30daysrange.Start, filter.ListView == TrackingUnitListView.LAST_30_DAYS);
       
    }
}
