using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;

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
             .Where(filter.Keyword, !string.IsNullOrEmpty(filter.Keyword))
             //.Where(q => q.SNo.Contains(filter.Keyword) || q.SimCard.SimCardNo!.Contains(filter.Keyword), !string.IsNullOrEmpty(filter.Keyword))
             .Where(x => x.CustomerId.Equals(filter.CustomerId), !(filter.CustomerId.Equals(0) || filter.CustomerId.Equals(null)))
             .Where(x => x.UStatus == filter.UStatus, !filter.UStatus.Equals(UStatus.All))
             .Where(x => x.Created >= todayrange.Start && x.Created < todayrange.End.AddDays(1), filter.ListView == TrackingUnitListView.TODAY)
             .Where(x => x.Created >= last30daysrange.Start, filter.ListView == TrackingUnitListView.LAST_30_DAYS);
       
    }
}
