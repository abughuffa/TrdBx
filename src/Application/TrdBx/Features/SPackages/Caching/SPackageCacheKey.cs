namespace CleanArchitecture.Blazor.Application.Features.SPackages.Caching;
/// <summary>
/// Static class for managing cache keys and expiration for SPackage-related data.
/// </summary>
public static class SPackageCacheKey
{
    public const string GetAllCacheKey = "all-SPackages";

    public static IEnumerable<string> Tags => new string[] { "spackage" };
    //public static void Refresh()
    //{
    //    FusionCacheFactory.RemoveByTags(Tags);
    //}
}

