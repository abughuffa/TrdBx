// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.DataDiagnosises.Caching;



public static class DataDiagnosisCacheKey
{
    public static string GetPaginationCacheKey(string parameters)
    {
        return $"DataDiagnosisesCacheKey:DataDiagnosisesWithPaginationQuery,{parameters}";
    }
    public static IEnumerable<string> Tags => new string[] { "datadiagnosis" };
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

