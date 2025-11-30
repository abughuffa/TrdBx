namespace CleanArchitecture.Blazor.Application.Features.Customers.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for filtering Ccs by their ID.
/// </summary>
public class AvaliableParentsSpecification : Specification<Customer>
{
    public AvaliableParentsSpecification()
    {

        Query.Where(q => q.ParentId == null)
             .Where(q => q.IsAvaliable == true);
    }

}