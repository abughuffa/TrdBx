

namespace CleanArchitecture.Blazor.Application.Features.Vehicles.Caching;
/// <summary>
/// Static class for managing cache keys and expiration for Vehicle-related data.
/// </summary>
public static class VehicleCacheKey
{
    public const string GetAllCacheKey = "all-Vehicles";
    public static string GetPaginationCacheKey(string parameters) {
        return $"VehicleCacheKey:VehiclesWithPaginationQuery,{parameters}";
    }
    public static string GetExportCacheKey(string parameters) {
        return $"VehicleCacheKey:ExportCacheKey,{parameters}";
    }
    public static string GetByNameCacheKey(string parameters) {
        return $"VehicleCacheKey:GetByNameCacheKey,{parameters}";
    }
    public static string GetByIdCacheKey(string parameters) {
        return $"VehicleCacheKey:GetByIdCacheKey,{parameters}";
    }
    public static IEnumerable<string>? Tags => new string[] { "Vehicle" };
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

