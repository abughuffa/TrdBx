namespace CleanArchitecture.Blazor.Application.Features.WialonUnits.Specifications;
#nullable disable warnings
public enum WialonUnitListView
{
    [Description("All")]
    All,
    [Description("Units Not Exist On TrdBx")]
    UnitsNotExistOnTrdBx,
    [Description("Units With SimCard Not Exist On Libyana")]
    UnitsWithSimCardNotExistOnLibyana
}

public class WialonUnitAdvancedFilter : PaginationFilter
{
    public WialonUnitListView ListView { get; set; } = WialonUnitListView.All;
}

