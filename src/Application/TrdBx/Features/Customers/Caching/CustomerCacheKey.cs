namespace CleanArchitecture.Blazor.Application.Features.Customers.Caching;
/// <summary>
/// Static class for managing cache keys and expiration for Customer-related data.
/// </summary>
public static class CustomerCacheKey
{
    public const string GetAllCacheKey = "all-Customers";

    public const string GetAvaliableCustomersCacheKey = "avaliable-Customers";

    public const string GetAvaliableChildsCacheKey = "avaliable-Childs";

    public static string GetExportCacheKey(string parameters)
    {
        return $"CustomerCacheKey:ExportCacheKey,{parameters}";
    }
    public static string GetPaginationCacheKey(string parameters)
    {
        return $"CustomerCacheKey:CustomersWithPaginationQuery,{parameters}";
    }

    public static string GetByIdCacheKey(string parameters)
    {
        return $"CustomerCacheKey:GetByIdCacheKey,{parameters}";
    }

    public static string GetAvaliableChildsByParentId(string parameters)
    {
        return $"CustomerCacheKey:GetAvaliableChildsByParentId,{parameters}";
    }


    public static IEnumerable<string> Tags => new string[] { "customer" };
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

