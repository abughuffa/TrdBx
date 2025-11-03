namespace CleanArchitecture.Blazor.Application.Features.SimCards.Specifications;

#nullable disable warnings
/// <summary>
/// Specifies the different views available for the SimCard list.
/// </summary>
public enum SimCardListView
{
    [Description("All")]
    All,
    [Description("Created Toady")]
    TODAY,
    [Description("Created within the last 30 days")]
    LAST_30_DAYS
}
/// <summary>
/// A class for applying advanced filtering options to SimCard lists.
/// </summary>
public class SimCardAdvancedFilter: PaginationFilter
{
    public TimeSpan LocalTimezoneOffset { get; set; }
    public SimCardListView ListView { get; set; } = SimCardListView.All;
    //public UserProfile? CurrentUser { get; set; }
}