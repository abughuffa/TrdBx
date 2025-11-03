namespace CleanArchitecture.Blazor.Application.Features.TestCases.RenewTestCases.Caching;
/// <summary>
/// Static class for managing cache keys and expiration for RenewTestCase-related data.
/// </summary>
public static class RenewTestCaseCacheKey
{
    public const string GetAllCacheKey = "all-RenewTestCases";
    public static string GetPaginationCacheKey(string parameters) {
        return $"RenewTestCaseCacheKey:RenewTestCasesWithPaginationQuery,{parameters}";
    }
    public static IEnumerable<string> Tags => new string[] { "renewtestcase" };
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

