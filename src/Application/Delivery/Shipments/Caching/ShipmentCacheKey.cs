

namespace CleanArchitecture.Blazor.Application.Features.Shipments.Caching;
/// <summary>
/// Static class for managing cache keys and expiration for Shipment-related data.
/// </summary>
public static class ShipmentCacheKey
{
    public const string GetAllCacheKey = "all-Shipments";
    public static string GetPaginationCacheKey(string parameters) {
        return $"ShipmentCacheKey:ShipmentsWithPaginationQuery,{parameters}";
    }
    public static string GetExportCacheKey(string parameters) {
        return $"ShipmentCacheKey:ExportCacheKey,{parameters}";
    }
    public static string GetByNameCacheKey(string parameters) {
        return $"ShipmentCacheKey:GetByNameCacheKey,{parameters}";
    }
    public static string GetByIdCacheKey(string parameters) {
        return $"ShipmentCacheKey:GetByIdCacheKey,{parameters}";
    }
    public static IEnumerable<string>? Tags => new string[] { "Shipment" };
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

