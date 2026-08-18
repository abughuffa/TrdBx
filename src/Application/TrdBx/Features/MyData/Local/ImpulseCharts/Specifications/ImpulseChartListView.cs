namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.ImpulseCharts.Specifications;

public enum ImpulseChartListView
{

    [Description("Sim Cards ExpiryDate Time line")]
    SimCardsExpiryDate,
    [Description("Unit Sub. ExpiryDate Time line")]
    UnitSubExpiryDate
}

public class ImpulseChartAdvancedFilter : PaginationFilter
{
    public int? CustomerId { get; set; } = null; 
    public DateOnly? FromDate { get; set; } = null;
    public DateOnly? ToDate { get; set; } = null;
    public ImpulseChartListView ListView { get; set; } = ImpulseChartListView.SimCardsExpiryDate;
}



