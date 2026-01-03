
namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.Charts.Caching;



public static class ChartCacheKey
{
    public static string GetPaginationCacheKey(string parameters)
    {
        return $"ChartCacheKey:ChartQuery,{parameters}";
    }
    public static IEnumerable<string> Tags => new string[] { "chart" };
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

