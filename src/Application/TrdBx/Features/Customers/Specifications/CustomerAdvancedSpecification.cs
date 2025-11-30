namespace CleanArchitecture.Blazor.Application.Features.Customers.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for advanced filtering of Customers.
/// </summary>
public class CustomerAdvancedSpecification : Specification<Customer>
{
    public CustomerAdvancedSpecification(CustomerAdvancedFilter filter)
    {
        var today = DateTime.UtcNow;
        var todayrange = today.GetDateRange(CustomerListView.TODAY.ToString());
        var last30daysrange = today.GetDateRange(CustomerListView.LAST_30_DAYS.ToString());

        Query.Where(q => q.Name != null)
             .Where(filter.Keyword, !string.IsNullOrEmpty(filter.Keyword))
             .Where(x => x.Created >= todayrange.Start && x.Created < todayrange.End.AddDays(1), filter.ListView == CustomerListView.TODAY)
             .Where(x => x.Created >= last30daysrange.Start, filter.ListView == CustomerListView.LAST_30_DAYS);

    }
}
