using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.Customers.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for filtering Ccs by their ID.
/// </summary>
public class AvaliableCustomersSpecification : Specification<Customer>
{
    public AvaliableCustomersSpecification()
    {
        Query.Where(c => c.ParentId != null)
             .Where(c=>(c.BillingPlan == BillingPlan.Basic)||(c.BillingPlan == BillingPlan.Advanced))
             .Where(q => q.IsAvaliable == true);
    }

}