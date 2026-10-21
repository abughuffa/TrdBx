using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.DataDiagnosises.Specifications;
#nullable disable warnings
public class DataDiagnosisAdvancedSpecification : Specification<DataDiagnosis>
{



    public DataDiagnosisAdvancedSpecification(DataDiagnosisAdvancedFilter filter)
    {


        Query.Where(q => q.UnitSNo != null || q.SimCardNo != null)
              .Where(q => q.UnitSNo!.Contains(filter.Keyword) || q.SimCardNo!.Contains(filter.Keyword), !string.IsNullOrEmpty(filter.Keyword))
              .Where(x => x.StatusOnTrdBx == filter.StatusOnTrdBx, filter.StatusOnTrdBx is not null)
              .Where(x => x.StatusOnWialon == filter.StatusOnWialon, filter.StatusOnWialon is not null)
              .Where(x => x.SimCardStatus == filter.SimCardStatus, filter.SimCardStatus is not null)
              .Where(x => x.LDOExpired == null || x.LDOExpired <= filter.ExpiersBefore, filter.ExpiersBefore is not null)



              ;

    }

}

