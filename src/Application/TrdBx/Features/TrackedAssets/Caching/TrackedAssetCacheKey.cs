namespace CleanArchitecture.Blazor.Application.Features.TrackedAssets.Caching;
/// <summary>
/// Static class for managing cache keys and expiration for TrackedAsset-related data.
/// </summary>
public static class TrackedAssetCacheKey
{
    public const string GetAllCacheKey = "all-TrackedAssets";

    public const string GetAvaliableCacheKey = "avaliable-TrackedAssets";

    public static string GetExportCacheKey(string parameters)
    {
        return $"TrackedAssetCacheKey:ExportCacheKey,{parameters}";
    }

    public static string GetPaginationCacheKey(string parameters) {
        return $"TrackedAssetCacheKey:TrackedAssetsWithPaginationQuery,{parameters}";
    }
    public static string GetByIdCacheKey(string parameters) {
        return $"TrackedAssetCacheKey:GetByIdCacheKey,{parameters}";
    }
    public static IEnumerable<string> Tags => new string[] { "trackedasset" };
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

