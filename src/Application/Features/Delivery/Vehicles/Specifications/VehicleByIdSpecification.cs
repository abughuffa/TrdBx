
namespace CleanArchitecture.Blazor.Application.Features.Vehicles.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for filtering Vehicles by their ID.
/// </summary>
public class VehicleByIdSpecification : Specification<Vehicle>
{
    public VehicleByIdSpecification(int id)
    {
       Query.Where(q => q.Id == id);
    }
}