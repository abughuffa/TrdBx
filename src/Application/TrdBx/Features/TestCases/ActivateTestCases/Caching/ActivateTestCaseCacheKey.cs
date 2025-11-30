
namespace CleanArchitecture.Blazor.Application.Features.TestCases.ActivateTestCases.Caching;
/// <summary>
/// Static class for managing cache keys and expiration for ActivateTestCase-related data.
/// </summary>
public static class ActivateTestCaseCacheKey
{
    public const string GetAllCacheKey = "all-ActivateTestCases";
    public static string GetPaginationCacheKey(string parameters) {
        return $"ActivateTestCaseCacheKey:ActivateTestCasesWithPaginationQuery,{parameters}";
    }
    public static IEnumerable<string> Tags => new string[] { "activatetestcase" };
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

