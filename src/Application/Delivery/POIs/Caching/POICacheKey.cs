

namespace CleanArchitecture.Blazor.Application.Features.POIs.Caching;
/// <summary>
/// Static class for managing cache keys and expiration for POI-related data.
/// </summary>
public static class POICacheKey
{
    public const string GetAllCacheKey = "all-POIs";
    public static string GetPaginationCacheKey(string parameters) {
        return $"POICacheKey:POIsWithPaginationQuery,{parameters}";
    }
    public static string GetExportCacheKey(string parameters) {
        return $"POICacheKey:ExportCacheKey,{parameters}";
    }
    public static string GetByNameCacheKey(string parameters) {
        return $"POICacheKey:GetByNameCacheKey,{parameters}";
    }
    public static string GetByIdCacheKey(string parameters) {
        return $"POICacheKey:GetByIdCacheKey,{parameters}";
    }
    public static IEnumerable<string>? Tags => new string[] { "POI" };
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

