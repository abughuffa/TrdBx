
namespace CleanArchitecture.Blazor.Application.Features.Warehouses.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for filtering Warehouses by their ID.
/// </summary>
public class WarehouseByIdSpecification : Specification<Warehouse>
{
    public WarehouseByIdSpecification(int id)
    {
       Query.Where(q => q.Id == id);
    }
}