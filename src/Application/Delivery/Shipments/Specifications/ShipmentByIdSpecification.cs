
namespace CleanArchitecture.Blazor.Application.Features.Shipments.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for filtering Shipments by their ID.
/// </summary>
public class ShipmentByIdSpecification : Specification<Shipment>
{
    public ShipmentByIdSpecification(int id)
    {
       Query.Where(q => q.Id == id);
    }
}