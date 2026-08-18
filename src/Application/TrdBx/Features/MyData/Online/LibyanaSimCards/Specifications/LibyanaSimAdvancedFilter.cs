namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.LibyanaSimCards.Specifications;
#nullable disable warnings
public enum LibyanaSimCardListView
{
    [Description("All")]
    All,
    [Description("SimCards Not Exist On TrdBx")]
    SimCardsNotExistOnTrdBx,
    [Description("SimCards Not Exist On Wialon")]
    SimCardsNotExistOnWialon,

}

public class LibyanaSimCardAdvancedFilter : PaginationFilter
{
    public LibyanaSimCardListView ListView { get; set; } = LibyanaSimCardListView.All;
}
