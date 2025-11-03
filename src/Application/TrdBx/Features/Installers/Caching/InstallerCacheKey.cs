namespace CleanArchitecture.Blazor.Application.Features.Installers.Caching;
/// <summary>
/// Static class for managing cache keys and expiration for Installer-related data.
/// </summary>
public static class InstallerCacheKey
{
    public const string GetAvaliableCacheKey = "avaliable-Installers";

    public static IEnumerable<string> Tags => new string[] { "installer" };
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

