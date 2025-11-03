using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.DbMatchings.Specifications;
#nullable disable warnings
public enum DbMatchingListView
{
    [Description("Matched By Unit And SimCard")]
    MatchedByUnitAndSimCard,
    [Description("Matched By SimCard Only")]
    MatchedBySimCardOnly,
    [Description("Matched By Unit Only")]
    MatchedByUnitOnly
}

public class DbMatchingAdvancedFilter : PaginationFilter
{
    public UStatus StatusOnTrdBx { get; set; } = UStatus.All;
    public WStatus StatusOnWialon { get; set; } = WStatus.All;
    public DbMatchingListView ListView { get; set; } = DbMatchingListView.MatchedByUnitAndSimCard;

}
