
namespace CleanArchitecture.Blazor.Application.Features.VehicleTypes.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for filtering VehicleTypes by their ID.
/// </summary>
public class VehicleTypeByIdSpecification : Specification<VehicleType>
{
    public VehicleTypeByIdSpecification(int id)
    {
       Query.Where(q => q.Id == id);
    }
}