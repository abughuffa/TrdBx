

namespace CleanArchitecture.Blazor.Application.Features.Warehouses.Caching;
/// <summary>
/// Static class for managing cache keys and expiration for Warehouse-related data.
/// </summary>
public static class WarehouseCacheKey
{
    public const string GetAllCacheKey = "all-Warehouses";
    public static string GetPaginationCacheKey(string parameters) {
        return $"WarehouseCacheKey:WarehousesWithPaginationQuery,{parameters}";
    }
    public static string GetExportCacheKey(string parameters) {
        return $"WarehouseCacheKey:ExportCacheKey,{parameters}";
    }
    public static string GetByNameCacheKey(string parameters) {
        return $"WarehouseCacheKey:GetByNameCacheKey,{parameters}";
    }
    public static string GetByIdCacheKey(string parameters) {
        return $"WarehouseCacheKey:GetByIdCacheKey,{parameters}";
    }
    public static IEnumerable<string>? Tags => new string[] { "Warehouse" };
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

