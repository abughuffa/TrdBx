// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Charts.Caching;



public static class ChartCacheKey
{
    public static string GetPaginationCacheKey(string parameters)
    {
        return $"ChartCacheKey:ChartQuery,{parameters}";
    }
    public static IEnumerable<string> Tags => new string[] { "chart" };
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

