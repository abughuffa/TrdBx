namespace CleanArchitecture.Blazor.Application.Features.TestCases.DeactivateTestCases.Caching;
/// <summary>
/// Static class for managing cache keys and expiration for DeactivateTestCase-related data.
/// </summary>
public static class DeactivateTestCaseCacheKey
{
    public const string GetAllCacheKey = "all-DeactivateTestCases";
    public static string GetPaginationCacheKey(string parameters) {
        return $"DeactivateTestCaseCacheKey:DeactivateTestCasesWithPaginationQuery,{parameters}";
    }
    public static IEnumerable<string> Tags => new string[] { "deactivatetestcase" };
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

