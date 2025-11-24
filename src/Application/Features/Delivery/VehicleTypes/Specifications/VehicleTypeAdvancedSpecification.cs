
namespace CleanArchitecture.Blazor.Application.Features.VehicleTypes.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for advanced filtering of VehicleTypes.
/// </summary>
public class VehicleTypeAdvancedSpecification : Specification<VehicleType>
{
    public VehicleTypeAdvancedSpecification(VehicleTypeAdvancedFilter filter)
    {


        Query.Where(q => q.Name != null)
             .Where(filter.Keyword,!string.IsNullOrEmpty(filter.Keyword));
       
    }
}
