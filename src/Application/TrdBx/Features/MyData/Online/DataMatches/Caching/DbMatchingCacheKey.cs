

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.DataMatches.Caching;



public static class DataMatchCacheKey
{

    public static string GetPaginationCacheKey(string parameters)
    {
        return $"DataMatchesCacheKey:DataMatchesWithPaginationQuery,{parameters}";
    }
    public static IEnumerable<string> Tags => new string[] { "datamatch" };
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

