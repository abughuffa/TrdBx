
namespace CleanArchitecture.Blazor.Application.Features.Warehouses.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for advanced filtering of Warehouses.
/// </summary>
public class WarehouseAdvancedSpecification : Specification<Warehouse>
{
    public WarehouseAdvancedSpecification(WarehouseAdvancedFilter filter)
    {


        Query.Where(q => q.Name != null)
             .Where(filter.Keyword,!string.IsNullOrEmpty(filter.Keyword))
             .Where(q => q.CreatedBy == filter.CurrentUser.UserId);
       
    }
}
