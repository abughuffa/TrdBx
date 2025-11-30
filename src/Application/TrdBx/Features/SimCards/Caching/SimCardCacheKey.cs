namespace CleanArchitecture.Blazor.Application.Features.SimCards.Caching;
/// <summary>
/// Static class for managing cache keys and expiration for SimCard-related data.
/// </summary>
public static class SimCardCacheKey
{
    public const string GetAllCacheKey = "all-SimCards";
    public const string GetAvaliableCacheKey = "avaliable-SimCards";
    public static string GetPaginationCacheKey(string parameters)
    {
        return $"SimCardCacheKey:SimCardsWithPaginationQuery,{parameters}";
    }
    public static string GetExportCacheKey(string parameters)
    {
        return $"SimCardCacheKey:ExportCacheKey,{parameters}";
    }
    public static string GetByNameCacheKey(string parameters)
    {
        return $"SimCardCacheKey:GetByNameCacheKey,{parameters}";
    }
    public static string GetByIdCacheKey(string parameters)
    {
        return $"SimCardCacheKey:GetByIdCacheKey,{parameters}";
    }

    public static string GetAvaliableWithIdsCacheKey(string parameters)
    {
        return $"SimCardCacheKey:GetAvaliableWithIdsCacheKey,{parameters}";
    }


    public static IEnumerable<string> Tags => new string[] { "simcard" };
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

