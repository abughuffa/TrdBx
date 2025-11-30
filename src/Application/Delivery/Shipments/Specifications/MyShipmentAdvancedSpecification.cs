
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Blazor.Application.Features.Shipments.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for advanced filtering of Shipments.
/// </summary>
public class MyShipmentAdvancedSpecification : Specification<Shipment>
{
    public MyShipmentAdvancedSpecification(ShipmentAdvancedFilter filter)
    {
        DateTime today = DateTime.UtcNow;
        var todayrange = today.GetDateRange(ShipmentListView.TODAY.ToString(), filter.LocalTimezoneOffset);
        var last7daysrange = today.GetDateRange(ShipmentListView.LAST_7_DAYS.ToString(),filter.LocalTimezoneOffset);

        switch (filter.CurrentUser.AssignedRoles)
        {
            case var x when (x.Contains("Admin")):
                Query.Where(q => q.ShipmentNo != null)
                     .Where(filter.Keyword, !string.IsNullOrEmpty(filter.Keyword))
                     .Where(x => x.Created >= todayrange.Start && x.Created < todayrange.End.AddDays(1), filter.ListView == ShipmentListView.TODAY)
                     .Where(x => x.Created >= last7daysrange.Start, filter.ListView == ShipmentListView.LAST_7_DAYS);
                break;

            case var x when (x.Contains("Trader")):
                Query.Where(q => q.ShipmentNo != null)
                     .Where(filter.Keyword, !string.IsNullOrEmpty(filter.Keyword))
                     .Where(q => q.CreatedBy == filter.CurrentUser.UserId)
                     .Where(x => x.Created >= todayrange.Start && x.Created < todayrange.End.AddDays(1), filter.ListView == ShipmentListView.TODAY)
                     .Where(x => x.Created >= last7daysrange.Start, filter.ListView == ShipmentListView.LAST_7_DAYS);
                break;

            case var x when (x.Contains("Transporter")):
                Query.Where(q => q.ShipmentNo != null)
                     .Where(filter.Keyword, !string.IsNullOrEmpty(filter.Keyword))
                     //.Where(q => q.StartLocation) // near me!
                     .Where(x => x.Created >= todayrange.Start && x.Created < todayrange.End.AddDays(1), filter.ListView == ShipmentListView.TODAY)
                     .Where(x => x.Created >= last7daysrange.Start, filter.ListView == ShipmentListView.LAST_7_DAYS);
                break;

            default:
                     Query.Where(q => q.CreatedBy == null); // Default case when none match
                     break;
        }




        
       
    }
}
