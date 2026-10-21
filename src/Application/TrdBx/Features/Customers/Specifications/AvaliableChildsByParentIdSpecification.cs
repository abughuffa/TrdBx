using CleanArchitecture.Blazor.Domain.Enums;
namespace CleanArchitecture.Blazor.Application.Features.Customers.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for filtering Ccs by their ID.
/// </summary>
public class AvaliableChildsByParentIdSpecification : Specification<Customer>
{

    public AvaliableChildsByParentIdSpecification(int parentId)
    {
        Query
        
        .Where(q => q.ParentId == parentId 
                         && q.BillingPlan == BillingPlan.Advanced 
                         && q.IsAvailable,parentId > 0);
    }     
    

}