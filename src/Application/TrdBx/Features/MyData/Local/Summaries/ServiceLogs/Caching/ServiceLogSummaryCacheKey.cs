namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.Summaries.ServiceLogs.Caching;

public static class ServiceLogSummaryCacheKey
{
    public const string GetCacheKey = "get-ServiceLogSummary";

    public static IEnumerable<string> Tags => new string[] { "servicelogsummary" };
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

