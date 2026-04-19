
namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.ImpulseCharts.Caching;



public static class ImpulseChartCacheKey
{
    public static string GetPaginationCacheKey(string parameters)
    {
        return $"ImpulseChartCacheKey:ImpulseChartQuery,{parameters}";
    }
    public static IEnumerable<string> Tags => new string[] { "chart" };
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

