
namespace CleanArchitecture.Blazor.Application.Features.Vehicles.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for advanced filtering of Vehicles.
/// </summary>
public class MyVehicleAdvancedSpecification : Specification<Vehicle>
{
    public MyVehicleAdvancedSpecification(VehicleAdvancedFilter filter)
    {

        Query.Where(q => q.VehicleNo != null)
             .Where(filter.Keyword,!string.IsNullOrEmpty(filter.Keyword))
             .Where(q => q.CreatedBy == filter.CurrentUser.UserId);
       
    }
}
