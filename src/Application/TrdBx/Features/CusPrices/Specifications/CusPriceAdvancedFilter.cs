namespace CleanArchitecture.Blazor.Application.Features.CusPrices.Specifications;

#nullable disable warnings
/// <summary>
/// Specifies the different views available for the CusPrice list.
/// </summary>
public enum CusPriceListView
{
    [Description("All")]
    All,
    [Description("Created Toady")]
    TODAY,
    [Description("Created within the last 30 days")]
    LAST_30_DAYS
}
/// <summary>
/// A class for applying advanced filtering options to CusPrice lists.
/// </summary>
public class CusPriceAdvancedFilter : PaginationFilter
{
    public int? CustomerId { get; set; }
    public CusPriceListView ListView { get; set; } = CusPriceListView.All;
    //public UserProfile? CurrentUser { get; set; }
}