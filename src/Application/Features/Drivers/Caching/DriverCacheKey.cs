
namespace CleanArchitecture.Blazor.Application.Features.Drivers.Caching;
/// <summary>
/// Static class for managing cache keys and expiration for Contact-related data.
/// </summary>
public static class DriverCacheKey
{
    public static string GetMyCacheKey(string parameters)
    {
        return $"DriverCacheKey:MyDriversQuery,{parameters}";
    }
    public static IEnumerable<string>? Tags => new string[] { "Driver" };
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

