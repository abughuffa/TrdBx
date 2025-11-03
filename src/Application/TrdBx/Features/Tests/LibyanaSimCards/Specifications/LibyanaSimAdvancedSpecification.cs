using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.LibyanaSimCards.Specifications;
#nullable disable warnings
public class LibyanaSimCardAdvancedSpecification : Specification<LibyanaSimCard>
{
    public LibyanaSimCardAdvancedSpecification(LibyanaSimCardAdvancedFilter filter)
    {


        Query.Where(q => q.SimCardNo != null)
             .Where(q => q.SimCardNo!.Contains(filter.Keyword), !string.IsNullOrEmpty(filter.Keyword));

    }
}
