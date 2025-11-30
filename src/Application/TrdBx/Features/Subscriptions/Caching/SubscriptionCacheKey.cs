namespace CleanArchitecture.Blazor.Application.Features.Subscriptions.Caching;
/// <summary>
/// Static class for managing cache keys and expiration for Subscription-related data.
/// </summary>
public static class SubscriptionCacheKey
{
    public const string GetAllCacheKey = "all-Subscriptions";
    public static string GetPaginationCacheKey(string parameters)
    {
        return $"SubscriptionCacheKey:SubscriptionsWithPaginationQuery,{parameters}";
    }
    public static string GetExportCacheKey(string parameters)
    {
        return $"SubscriptionCacheKey:ExportCacheKey,{parameters}";
    }
    public static string GetByNameCacheKey(string parameters)
    {
        return $"SubscriptionCacheKey:GetByNameCacheKey,{parameters}";
    }
    public static string GetByIdCacheKey(string parameters)
    {
        return $"SubscriptionCacheKey:GetByIdCacheKey,{parameters}";
    }
    public static string GetByUnitIdCacheKey(string parameters)
    {
        return $"SubscriptionCacheKey:GetByUnitIdCacheKey,{parameters}";


    }
    public static IEnumerable<string> Tags => new string[] { "gpsunitsubscription" };
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

