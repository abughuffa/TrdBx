namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
/// <summary>
/// Static class for managing cache keys and expiration for TrackingUnit-related data.
/// </summary>
public static class TrackingUnitCacheKey
{
    public const string GetAllCacheKey = "all-TrackingUnits";

    public const string GetAvaliableCacheKey = "avaliable-TrackingUnits";

    public static string GetExportCacheKey(string parameters)
    {
        return $"TrackingUnitCacheKey:ExportCacheKey,{parameters}";
    }
    public static string GetPaginationCacheKey(string parameters) {
        return $"TrackingUnitCacheKey:TrackingUnitsWithPaginationQuery,{parameters}";
    }

    public static string GetByNameCacheKey(string parameters) {
        return $"TrackingUnitCacheKey:GetByNameCacheKey,{parameters}";
    }
    public static string GetByIdCacheKey(string parameters) {
        return $"TrackingUnitCacheKey:GetByIdCacheKey,{parameters}";
    }

    public static IEnumerable<string> Tags => new string[] { "gpsunit" };


    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

