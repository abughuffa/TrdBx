namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Specifications;

#nullable disable warnings
/// <summary>
/// Specifies the different views available for the TrackingUnit list.
/// </summary>
public enum TrackingUnitListView
{
    [Description("All")]
    All,
    [Description("Created Toady")]
    TODAY,
    [Description("Created within the last 30 days")]
    LAST_30_DAYS
}
/// <summary>
/// A class for applying advanced filtering options to TrackingUnit lists.
/// </summary>
public class TrackingUnitAdvancedFilter: PaginationFilter
{
    public TimeSpan LocalTimezoneOffset { get; set; }
    //public UserProfile? CurrentUser { get; set; }
    public TrackingUnitListView ListView { get; set; } = TrackingUnitListView.All;

}