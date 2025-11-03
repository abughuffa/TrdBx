using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.Diagnostics.Specifications;
#nullable disable warnings
public class DiagnosticAdvancedSpecification : Specification<Diagnostic>
{



    public DiagnosticAdvancedSpecification(DiagnosticAdvancedFilter filter)
    {


        Query.Where(q => q.UnitSNo != null || q.SimCardNo != null)
              .Where(q => q.UnitSNo!.Contains(filter.Keyword) || q.SimCardNo!.Contains(filter.Keyword), !string.IsNullOrEmpty(filter.Keyword))
              .Where(x => x.StatusOnTrdBx == filter.StatusOnTrdBx, filter.StatusOnTrdBx != UStatus.All)
              .Where(x => x.StatusOnWialon == filter.StatusOnWialon, filter.StatusOnWialon != WStatus.All)
              .Where(x => x.SimCardStatus == filter.SimCardStatus, filter.SimCardStatus != SLStatus.All)
              .Where(x => x.LDOExpired == null || x.LDOExpired <= filter.ExpiersBefore, !(filter.ExpiersBefore is null))



              ;

    }

}

