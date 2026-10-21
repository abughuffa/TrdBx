namespace CleanArchitecture.Blazor.Application.Features.SmsMessages.Caching;
/// <summary>
/// Static class for managing cache keys and expiration for Invoice-related data.
/// </summary>
public static class SmsMessageCacheKey
{
    public const string GetAllCacheKey = "all-SmsMessages";
    public static string GetPaginationCacheKey(string parameters)
    {
        return $"SmsMessageCacheKey:SmsMessagesWithPaginationQuery,{parameters}";
    }
    public static string GetExportCacheKey(string parameters)
    {
        return $"SmsMessageCacheKey:ExportCacheKey,{parameters}";
    }
    public static string GetByIdCacheKey(string parameters)
    {
        return $"SmsMessageCacheKey:GetByIdCacheKey,{parameters}";
    }
    public static IEnumerable<string> Tags => new string[] { "smsmessage" };
}

