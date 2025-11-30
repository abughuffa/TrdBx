

namespace CleanArchitecture.Blazor.Application.Features.VehicleTypes.Caching;
/// <summary>
/// Static class for managing cache keys and expiration for VehicleType-related data.
/// </summary>
public static class VehicleTypeCacheKey
{
    public const string GetAllCacheKey = "all-VehicleTypes";
    public static string GetPaginationCacheKey(string parameters) {
        return $"VehicleTypeCacheKey:VehicleTypesWithPaginationQuery,{parameters}";
    }
    public static string GetExportCacheKey(string parameters) {
        return $"VehicleTypeCacheKey:ExportCacheKey,{parameters}";
    }
    public static string GetByNameCacheKey(string parameters) {
        return $"VehicleTypeCacheKey:GetByNameCacheKey,{parameters}";
    }
    public static string GetByIdCacheKey(string parameters) {
        return $"VehicleTypeCacheKey:GetByIdCacheKey,{parameters}";
    }
    public static IEnumerable<string>? Tags => new string[] { "VehicleType" };
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

