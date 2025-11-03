namespace CleanArchitecture.Blazor.Application.Features.Summaries.SimCards.Caching;

public static class SimCardSummaryCacheKey
{
    public const string GetCacheKey = "get-SimCardSummary";

    public static IEnumerable<string> Tags => new string[] { "simsummary" };
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

