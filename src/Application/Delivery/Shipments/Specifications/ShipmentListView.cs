
namespace CleanArchitecture.Blazor.Application.Features.Shipments.Specifications;

#nullable disable warnings
/// <summary>
/// Specifies the different views available for the Shipment list.
/// </summary>
public enum ShipmentListView
{
    [Description("All")]
    All,
    [Description("Created Toady")]
    TODAY,
    [Description("Created within the last 7 days")]
    LAST_7_DAYS
}
/// <summary>
/// A class for applying advanced filtering options to Shipment lists.
/// </summary>
public class ShipmentAdvancedFilter: PaginationFilter
{
    public TimeSpan LocalTimezoneOffset { get; set; }
    public ShipmentListView ListView { get; set; } = ShipmentListView.All;
    public UserProfile? CurrentUser { get; set; }
}