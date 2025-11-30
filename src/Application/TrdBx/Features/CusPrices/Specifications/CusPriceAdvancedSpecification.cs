namespace CleanArchitecture.Blazor.Application.Features.CusPrices.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for advanced filtering of CusPrices.
/// </summary>
public class CusPriceAdvancedSpecification : Specification<CusPrice>
{
    public CusPriceAdvancedSpecification(CusPriceAdvancedFilter filter)
    {
        var today = DateTime.UtcNow;
        var todayrange = today.GetDateRange(CusPriceListView.TODAY.ToString());
        var last30daysrange = today.GetDateRange(CusPriceListView.LAST_30_DAYS.ToString());

        Query.Where(q => q.Price != 0.0m)
             .Where(filter.Keyword, !string.IsNullOrEmpty(filter.Keyword))
             .Where(q => q.CustomerId == filter.CustomerId, !(filter.CustomerId == null))
             .Where(x => x.Created >= todayrange.Start && x.Created < todayrange.End.AddDays(1), filter.ListView == CusPriceListView.TODAY)
             .Where(x => x.Created >= last30daysrange.Start, filter.ListView == CusPriceListView.LAST_30_DAYS);

    }
}
