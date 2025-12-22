
namespace CleanArchitecture.Blazor.Application.Features.POIs.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for advanced filtering of POIs.
/// </summary>
public class POIAdvancedSpecification : Specification<POI>
{
    public POIAdvancedSpecification(POIAdvancedFilter filter)
    {


        Query.Where(q => q.Name != null)
             .Where(filter.Keyword,!string.IsNullOrEmpty(filter.Keyword))
             .Where(q => q.CreatedBy == filter.CurrentUser.UserId);
       
    }
}
