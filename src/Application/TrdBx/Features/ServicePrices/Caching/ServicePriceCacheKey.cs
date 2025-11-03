namespace CleanArchitecture.Blazor.Application.Features.ServicePrices.Caching;
/// <summary>
/// Static class for managing cache keys and expiration for ServicePrice-related data.
/// </summary>
public static class ServicePriceCacheKey
{
    public const string GetAllCacheKey = "all-ServicePrices";

    
    public static string GetPaginationCacheKey(string parameters) {
        return $"ServicePriceCacheKey:ServicePricesWithPaginationQuery,{parameters}";
    }
    public static string GetExportCacheKey(string parameters) {
        return $"ServicePriceCacheKey:ExportCacheKey,{parameters}";
    }
    public static string GetByNameCacheKey(string parameters) {
        return $"ServicePriceCacheKey:GetByNameCacheKey,{parameters}";
    }
    public static string GetByIdCacheKey(string parameters) {
        return $"ServicePriceCacheKey:GetByIdCacheKey,{parameters}";
    }
    public static IEnumerable<string> Tags => new string[] { "serviceprice" };
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

