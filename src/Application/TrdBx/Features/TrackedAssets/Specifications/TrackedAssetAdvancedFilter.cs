namespace CleanArchitecture.Blazor.Application.Features.TrackedAssets.Specifications;

public enum TrackedAssetListView
{
    [Description("All")]
    All,
    [Description("Created Toady")]
    TODAY,
    [Description("Created within the last 30 days")]
    LAST_30_DAYS
}
/// <summary>
/// A class for applying advanced filtering options to TrackedAsset lists.
/// </summary>
public class TrackedAssetAdvancedFilter: PaginationFilter
{
    public TrackedAssetListView ListView { get; set; } = TrackedAssetListView.All;
    //public UserProfile? CurrentUser { get; set; }
}