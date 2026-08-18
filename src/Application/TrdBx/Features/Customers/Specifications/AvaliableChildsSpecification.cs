namespace CleanArchitecture.Blazor.Application.Features.Customers.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for filtering Ccs by their ID.
/// </summary>
public class AvaliableChildsSpecification : Specification<Customer>
{
    public AvaliableChildsSpecification()
    {

        Query.Where(q => q.ParentId != null)
             .Where(q => q.IsAvaliable == true);
    }

}