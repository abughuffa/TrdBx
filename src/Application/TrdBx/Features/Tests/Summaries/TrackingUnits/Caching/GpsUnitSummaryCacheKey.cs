namespace CleanArchitecture.Blazor.Application.Features.Summaries.TrackingUnits.Caching;

public static class TrackingUnitSummaryCacheKey
{
    public const string GetCacheKey = "get-TrackingUnitSummary";

    public static IEnumerable<string> Tags => new string[] { "gpsunitsummary" };
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

