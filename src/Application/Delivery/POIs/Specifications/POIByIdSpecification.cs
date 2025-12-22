
namespace CleanArchitecture.Blazor.Application.Features.POIs.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for filtering POIs by their ID.
/// </summary>
public class POIByIdSpecification : Specification<POI>
{
    public POIByIdSpecification(int id)
    {
       Query.Where(q => q.Id == id);
    }
}