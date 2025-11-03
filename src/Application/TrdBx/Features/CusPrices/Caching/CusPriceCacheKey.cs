namespace CleanArchitecture.Blazor.Application.Features.CusPrices.Caching;
/// <summary>
/// Static class for managing cache keys and expiration for CusPrice-related data.
/// </summary>
public static class CusPriceCacheKey
{
    public const string GetAllCacheKey = "all-CusPrices";

    public static string GetExportCacheKey(string parameters)
    {
        return $"CusPriceCacheKey:ExportCacheKey,{parameters}";
    }
    public static string GetPaginationCacheKey(string parameters)
    {
        return $"CusPriceCacheKey:CusPricesWithPaginationQuery,{parameters}";
    }

    public static string GetByIdCacheKey(string parameters)
    {
        return $"CusPriceCacheKey:GetByIdCacheKey,{parameters}";
    }


    public static IEnumerable<string> Tags => new string[] { "cusprice" };
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

