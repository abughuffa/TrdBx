using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.DataDiagnosises.Specifications;
#nullable disable warnings
public enum DataDiagnosisListView
{

    [Description("SimCards of Units Which are Exist On TrdBx & Wialon")]
    SimCardsOfUnitsWhichAreExistOnTrdBxAndWialon,
    [Description("SimCards of Units Which are not Exist On Wialon")]
    SimCardsOfUnitsWhichAreNotExistOnWialon,
    [Description("SimCards of Units Which are not Exist On TrdBx")]
    SimCardsOfUnitsWhichAreNotExistOnTrdBx,
    [Description("SimCards of Units Which are not Exist On TrdBx or Wialon")]
    SimCardsOfUnitsWhichAreNotExistOnTrdBxOrWialon
}

public class DataDiagnosisAdvancedFilter : PaginationFilter
{
    public UStatus? StatusOnTrdBx { get; set; } = null;
    public WStatus? StatusOnWialon { get; set; } = null;
    public SLStatus? SimCardStatus { get; set; } = null;
    public DateTime? ExpiersBefore { get; set; } = null;
    public DataDiagnosisListView ListView { get; set; } = DataDiagnosisListView.SimCardsOfUnitsWhichAreExistOnTrdBxAndWialon;

}



