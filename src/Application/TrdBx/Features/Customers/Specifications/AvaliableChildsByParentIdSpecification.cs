namespace CleanArchitecture.Blazor.Application.Features.Customers.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for filtering Ccs by their ID.
/// </summary>
public class AvaliableChildsByParentIdSpecification : Specification<Customer>
{
    public AvaliableChildsByParentIdSpecification(int? Id)
    {
        if (Id is null)
            Query.Where(q => q.ParentId != null);
        else
            Query.Where(q => q.ParentId == Id);
    }

}