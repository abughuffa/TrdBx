// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.DbMatchings.Caching;



public static class DbMatchingCacheKey
{

    public static string GetPaginationCacheKey(string parameters)
    {
        return $"DbMatchingsCacheKey:DbMatchingsWithPaginationQuery,{parameters}";
    }
    public static IEnumerable<string> Tags => new string[] { "dbmatching" };
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

