

using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.TrackedAssets.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for advanced filtering of TrackedAssets.
/// </summary>
public class TrackedAssetAdvancedSpecification : Specification<TrackedAsset>
{
    public TrackedAssetAdvancedSpecification(TrackedAssetAdvancedFilter filter)
    {
        var today = DateTime.UtcNow;
        var todayrange = today.GetDateRange(TrackedAssetListView.TODAY.ToString());
        var last30daysrange = today.GetDateRange(TrackedAssetListView.LAST_30_DAYS.ToString());

        Query.Where(q => q.TrackedAssetNo != null)
            .Where(x => x.TrackedAssetNo.Contains(filter.Keyword) || x.TrackedAssetCode.Contains(filter.Keyword) ||
                        x.PlateNo.Contains(filter.Keyword) || x.VinSerNo.Contains(filter.Keyword) 
                        || x.OldVehicleNo.Contains(filter.Keyword), !string.IsNullOrEmpty(filter.Keyword))
             .Where(filter.Keyword,!string.IsNullOrEmpty(filter.Keyword))
             //.Where(q => q.CreatedBy == filter.CurrentUser.UserId, filter.ListView == TrackedAssetListView.My && filter.CurrentUser is not null)
             .Where(x => x.Created >= todayrange.Start && x.Created < todayrange.End.AddDays(1), filter.ListView == TrackedAssetListView.TODAY)
             .Where(x => x.Created >= last30daysrange.Start, filter.ListView == TrackedAssetListView.LAST_30_DAYS);
       
    }
}
