namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateHostingTestCases.Caching;
/// <summary>
/// Static class for managing cache keys and expiration for ActivateHostingTestCase-related data.
/// </summary>
public static class ActivateHostingTestCaseCacheKey
{
    public const string GetAllCacheKey = "all-ActivateHostingTestCases";
    public static string GetPaginationCacheKey(string parameters) {
        return $"ActivateHostingTestCaseCacheKey:ActivateHostingTestCasesWithPaginationQuery,{parameters}";
    }
    public static IEnumerable<string> Tags => new string[] { "activatehostingtestcase" };
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

