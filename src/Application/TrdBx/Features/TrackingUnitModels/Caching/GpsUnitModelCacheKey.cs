namespace CleanArchitecture.Blazor.Application.Features.TrackingUnitModels.Caching;
/// <summary>
/// Static class for managing cache keys and expiration for TrackingUnitModel-related data.
/// </summary>
public static class TrackingUnitModelCacheKey
{
    public const string GetAllCacheKey = "all-TrackingUnitModels";
    public static IEnumerable<string> Tags => new string[] { "gpsunitmodel" };
    //public static void Refresh()
    //{
    //    FusionCacheFactory.RemoveByTags(Tags);
    //}
}

