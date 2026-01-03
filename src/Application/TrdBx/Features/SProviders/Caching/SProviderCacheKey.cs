namespace CleanArchitecture.Blazor.Application.Features.SProviders.Caching;
/// <summary>
/// Static class for managing cache keys and expiration for SProvider-related data.
/// </summary>
public static class SProviderCacheKey
{
    public const string GetAllCacheKey = "all-SProviders";

    public static IEnumerable<string> Tags => new string[] { "sprovider" };


    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

