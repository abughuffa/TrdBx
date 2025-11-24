
namespace CleanArchitecture.Blazor.Application.Features.Shipments.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for advanced filtering of Shipments.
/// </summary>
public class ShipmentAdvancedSpecification : Specification<Shipment>
{
    public ShipmentAdvancedSpecification(ShipmentAdvancedFilter filter)
    {
        DateTime today = DateTime.UtcNow;
        var todayrange = today.GetDateRange(ShipmentListView.TODAY.ToString(), filter.LocalTimezoneOffset);
        var last7daysrange = today.GetDateRange(ShipmentListView.LAST_7_DAYS.ToString(),filter.LocalTimezoneOffset);

        Query.Where(q => q.ShipmentNo != null)
             .Where(filter.Keyword,!string.IsNullOrEmpty(filter.Keyword))
             .Where(q => q.CreatedBy == filter.CurrentUser.UserId)
             .Where(x => x.Created >= todayrange.Start && x.Created < todayrange.End.AddDays(1), filter.ListView == ShipmentListView.TODAY)
             .Where(x => x.Created >= last7daysrange.Start, filter.ListView == ShipmentListView.LAST_7_DAYS);
       
    }
}
