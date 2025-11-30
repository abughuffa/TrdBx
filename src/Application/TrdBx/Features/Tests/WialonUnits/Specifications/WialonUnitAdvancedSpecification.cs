using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.WialonUnits.Specifications;
#nullable disable warnings
public class WialonUnitAdvancedSpecification : Specification<WialonUnit>
{
    public WialonUnitAdvancedSpecification(WialonUnitAdvancedFilter filter)
    {


        Query.Where(q => q.UnitSNo != null || q.SimCardNo != null)
            .Where(q => q.UnitSNo!.Contains(filter.Keyword) || q.SimCardNo!.Contains(filter.Keyword), !string.IsNullOrEmpty(filter.Keyword));

    }
}
