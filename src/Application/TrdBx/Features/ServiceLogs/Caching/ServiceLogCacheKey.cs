namespace CleanArchitecture.Blazor.Application.Features.ServiceLogs.Caching;
/// <summary>
/// Static class for managing cache keys and expiration for ServiceLog-related data.
/// </summary>
public static class ServiceLogCacheKey
{
    public const string GetAllCacheKey = "all-ServiceLogs";

    public const string GetAvaliableCacheKey = "avaliable-ServiceLogs";
    
    public static string GetPaginationCacheKey(string parameters) {
        return $"ServiceLogCacheKey:ServiceLogsWithPaginationQuery,{parameters}";
    }
    public static string GetExportCacheKey(string parameters) {
        return $"ServiceLogCacheKey:ExportCacheKey,{parameters}";
    }
    public static string GetByNameCacheKey(string parameters) {
        return $"ServiceLogCacheKey:GetByNameCacheKey,{parameters}";
    }
    public static string GetByIdCacheKey(string parameters) {
        return $"ServiceLogCacheKey:GetByIdCacheKey,{parameters}";
    }
    public static IEnumerable<string> Tags => new string[] { "servicelog" };
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

