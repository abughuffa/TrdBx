// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Diagnostics.Caching;



public static class DiagnosticCacheKey
{
    public static string GetPaginationCacheKey(string parameters)
    {
        return $"DiagnosticsCacheKey:DiagnosticsWithPaginationQuery,{parameters}";
    }
    public static IEnumerable<string> Tags => new string[] { "diagnostic" };
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

