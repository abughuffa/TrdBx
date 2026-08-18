using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.Customers.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for filtering Ccs by their ID.
/// </summary>
public class AvaliableCustomersSpecification : Specification<Customer>
{
    public AvaliableCustomersSpecification(bool WithAdvParents)
    {
 
        if (WithAdvParents)     
            Query.Where(q => q.IsAvaliable == true);
        else
            Query.Where(c => (c.ParentId == null && c.BillingPlan == BillingPlan.Basic) || (c.ParentId != null && c.BillingPlan == BillingPlan.Advanced))
                .Where(c => c.IsAvaliable == true);
    }

}