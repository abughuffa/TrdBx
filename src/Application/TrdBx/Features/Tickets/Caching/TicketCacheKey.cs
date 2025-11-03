namespace CleanArchitecture.Blazor.Application.Features.Tickets.Caching;
/// <summary>
/// Static class for managing cache keys and expiration for Ticket-related data.
/// </summary>
public static class TicketCacheKey
{
    public const string GetAllCacheKey = "all-Tickets";

    public const string GetAvaliableCacheKey = "avaliable-Tickets";
    
    public static string GetPaginationCacheKey(string parameters) {
        return $"TicketCacheKey:TicketsWithPaginationQuery,{parameters}";
    }
    public static string GetByIdCacheKey(string parameters) {
        return $"TicketCacheKey:GetByIdCacheKey,{parameters}";
    }
    public static IEnumerable<string> Tags => new string[] { "ticket" };
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

