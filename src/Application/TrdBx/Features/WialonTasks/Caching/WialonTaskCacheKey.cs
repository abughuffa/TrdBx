namespace CleanArchitecture.Blazor.Application.Features.WialonTasks.Caching;
/// <summary>
/// Static class for managing cache keys and expiration for WialonTask-related data.
/// </summary>
public static class WialonTaskCacheKey
{
    public const string GetAllCacheKey = "all-WialonTasks";
    public static string GetPaginationCacheKey(string parameters)
    {
        return $"WialonTaskCacheKey:WialonTasksWithPaginationQuery,{parameters}";
    }
    public static string GetExportCacheKey(string parameters)
    {
        return $"WialonTaskCacheKey:ExportCacheKey,{parameters}";
    }
    public static string GetByNameCacheKey(string parameters)
    {
        return $"WialonTaskCacheKey:GetByNameCacheKey,{parameters}";
    }
    public static string GetByIdCacheKey(string parameters)
    {
        return $"WialonTaskCacheKey:GetByIdCacheKey,{parameters}";
    }

    public static string GetByServiceLogIdCacheKey(string parameters)
    {
        return $"WialonTaskCacheKey:GetByServiceLogIdCacheKey,{parameters}";
    }

    public static IEnumerable<string> Tags => new string[] { "wialontask" };
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

