// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.WialonUnits.Caching;

public static class WialonUnitCacheKey
{

    public const string GetAllCacheKey = "all-WialonUnits";

    public static string GetPaginationCacheKey(string parameters)
    {
        return $"WialonUnitCacheKey:WialonUnitsWithPaginationQuery,{parameters}";
    }
    public static string GetByIdCacheKey(string parameters)
    {
        return $"WialonUnitCacheKey:GetByIdCacheKey,{parameters}";
    }

    public static IEnumerable<string> Tags => new string[] { "wialonunit" };
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

