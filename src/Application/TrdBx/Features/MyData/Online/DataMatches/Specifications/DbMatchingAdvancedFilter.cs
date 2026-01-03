using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.DataMatches.Specifications;
#nullable disable warnings
public enum DataMatchListView
{
    [Description("Matched By Unit And SimCard")]
    MatchedByUnitAndSimCard,
    [Description("Matched By SimCard Only")]
    MatchedBySimCardOnly,
    [Description("Matched By Unit Only")]
    MatchedByUnitOnly
}

public class DataMatchAdvancedFilter : PaginationFilter
{
    public UStatus StatusOnTrdBx { get; set; } = UStatus.All;
    public WStatus StatusOnWialon { get; set; } = WStatus.All;
    public DataMatchListView ListView { get; set; } = DataMatchListView.MatchedByUnitAndSimCard;

}
