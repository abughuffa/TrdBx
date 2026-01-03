namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.Summaries.SimCards.Caching;

public static class SimCardSummaryCacheKey
{
    public const string GetCacheKey = "get-SimCardSummary";

    public static IEnumerable<string> Tags => new string[] { "simsummary" };
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

