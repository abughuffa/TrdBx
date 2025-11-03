namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateGprsTestCases.Caching;
/// <summary>
/// Static class for managing cache keys and expiration for ActivateGprsTestCase-related data.
/// </summary>
public static class ActivateGprsTestCaseCacheKey
{
    public const string GetAllCacheKey = "all-ActivateGprsTestCases";
    public static string GetPaginationCacheKey(string parameters) {
        return $"ActivateGprsTestCaseCacheKey:ActivateGprsTestCasesWithPaginationQuery,{parameters}";
    }
    public static IEnumerable<string> Tags => new string[] { "activategprstestcase" };
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

