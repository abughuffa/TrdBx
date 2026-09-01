using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.Customers.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for filtering Ccs by their ID.
/// </summary>
public class AvaliableCustomersSpecification : Specification<Customer>
{
    public AvaliableCustomersSpecification(bool withAdvParents)
    {   
          if (withAdvParents)
        {
            // Only root customers with Basic plan
            Query.Where(c => c.IsAvailable);
        }
        else
        {
            // Include BOTH: 
            // 1. Root customers with Basic plan
            // 2. Child customers with Advanced plan (regardless of parent)
            Query.Where(c => c.IsAvailable && 
                                   ((c.ParentId == null && c.BillingPlan == BillingPlan.Basic) ||
                                    (c.ParentId != null && c.BillingPlan == BillingPlan.Advanced)));

        }
            // Query
            //      .Where(c => (c.ParentId == null && c.BillingPlan == BillingPlan.Basic)
            //                  ||
            //                  (c.ParentId != null && c.BillingPlan == BillingPlan.Advanced)
            //                  , withAdvParents == true);
    }
}