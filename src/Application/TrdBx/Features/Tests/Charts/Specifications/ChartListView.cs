namespace CleanArchitecture.Blazor.Application.Features.Charts.Specifications;

public enum ChartListView
{

    [Description("Sim Cards ExpiryDate Time line")]
    SimCardsExpiryDate,
    [Description("Unit Sub. ExpiryDate Time line")]
    UnitSubExpiryDate
}

public class ChartAdvancedFilter : PaginationFilter
{
    public int CustomerId { get; set; } = 0; 
    public DateOnly? FromDate { get; set; } = null;
    public DateOnly? ToDate { get; set; } = null;
    public ChartListView ListView { get; set; } = ChartListView.SimCardsExpiryDate;
}



