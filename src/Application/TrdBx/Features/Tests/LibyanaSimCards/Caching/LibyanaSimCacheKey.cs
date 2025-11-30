// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.LibyanaSimCards.Caching;

public static class LibyanaSimCardCacheKey
{

    public const string GetAllCacheKey = "all-LibyanaSimCards";

    public static string GetPaginationCacheKey(string parameters)
    {
        return $"LibyanaSimCardCacheKey:LibyanaSimCardsWithPaginationQuery,{parameters}";
    }
    public static IEnumerable<string> Tags => new string[] { "libyanasim" };
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

