using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.WialonUnits.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for filtering Assets by their ID.
/// </summary>
public class WialonUnitByIdSpecification : Specification<WialonUnit>
{
    public WialonUnitByIdSpecification(int id)
    {
       Query.Where(q => q.Id == id);
    }
}