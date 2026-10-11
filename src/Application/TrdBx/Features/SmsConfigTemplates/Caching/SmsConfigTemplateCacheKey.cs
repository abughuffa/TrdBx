namespace CleanArchitecture.Blazor.Application.Features.SmsConfigTemplates.Caching;
/// <summary>
/// Static class for managing cache keys and expiration for TrackingUnit-related data.
/// </summary>
public static class SmsConfigTemplateCacheKey
{
    public const string GetAllCacheKey = "all-SmsConfigTemplates";


    public static string GetExportCacheKey(string parameters)
    {
        return $"SmsConfigTemplatesCacheKey:ExportCacheKey,{parameters}";
    }
    public static string GetPaginationCacheKey(string parameters) {
        return $"SmsConfigTemplatesCacheKey:SmsConfigTemplatesWithPaginationQuery,{parameters}";
    }

    public static string GetByNameCacheKey(string parameters) {
        return $"SmsConfigTemplateCacheKey:GetByNameCacheKey,{parameters}";
    }
    public static string GetByIdCacheKey(string parameters) {
        return $"SmsConfigTemplateCacheKey:GetByIdCacheKey,{parameters}";
    }

      public static string GetByModelIdCacheKey(string parameters) {
        return $"SmsConfigTemplateCacheKey:GetByModelIdCacheKey,{parameters}";
    }

    public static string GetAvaliableSmsConfigTemplatesWithIdCacheKey(string parameters)
    {
        return $"SmsConfigTemplateCacheKey:GetAvaliableSmsConfigTemplatesWithIdCacheKey,{parameters}";
    }

    public static IEnumerable<string> Tags => new string[] { "smsconfigtemplates" };


    // public static void Refresh()
    // {
    //     FusionCacheFactory.RemoveByTags(Tags);
    // }
}

