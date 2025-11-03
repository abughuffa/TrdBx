using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.DbMatchings.Specifications;
#nullable disable warnings
public class DbMatchingAdvancedSpecification : Specification<DbMatching>
{



    public DbMatchingAdvancedSpecification(DbMatchingAdvancedFilter filter)
    {

        //
        Query.Where(q => q.WUnitSNo != null || q.TUnitSNo != null || q.WSimCardNo != null || q.TSimCardNo != null)
             .Where(q => q.WUnitSNo!.Contains(filter.Keyword)
                         || q.TUnitSNo!.Contains(filter.Keyword)
                         || q.WSimCardNo!.Contains(filter.Keyword)
                         || q.TSimCardNo!.Contains(filter.Keyword), !string.IsNullOrEmpty(filter.Keyword))
             .Where(x => x.StatusOnTrdBx == filter.StatusOnTrdBx, filter.StatusOnTrdBx != UStatus.All)
             .Where(x => x.StatusOnWialon == filter.StatusOnWialon, filter.StatusOnWialon != WStatus.All);

    }
}

