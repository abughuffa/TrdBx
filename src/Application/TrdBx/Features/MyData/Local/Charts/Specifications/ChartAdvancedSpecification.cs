using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.Charts.Specifications;
#nullable disable warnings
public class ChartAdvancedSpecification : Specification<TrackingUnit>
{



    public ChartAdvancedSpecification(ChartAdvancedFilter filter)
    {
        switch (filter.ListView)
        {
            case ChartListView.SimCardsExpiryDate:
                {
                    Query
                   .Where(x => x.CustomerId.Equals(filter.CustomerId), !(filter.CustomerId.Equals(0) || filter.CustomerId.Equals(null)))
                   .Where(x => x.SimCard.ExDate == null || x.SimCard.ExDate >= filter.FromDate, !(filter.FromDate is null))
                   .Where(x => x.SimCard.ExDate == null || x.SimCard.ExDate <= filter.ToDate, !(filter.ToDate is null))
                   .Where(x => x.UStatus == UStatus.InstalledActiveGprs
                                         || x.UStatus == UStatus.InstalledActiveHosting
                                         || x.UStatus == UStatus.InstalledActive);
                    break;
                }
            case ChartListView.UnitSubExpiryDate:
                {
                    Query
                   .Where(x => x.CustomerId.Equals(filter.CustomerId), !(filter.CustomerId.Equals(0) || filter.CustomerId.Equals(null)))
                   .Where(x => x.Subscriptions.First().SeDate == null || x.Subscriptions.First().SeDate >= filter.FromDate, !(filter.FromDate is null))
                   .Where(x => x.Subscriptions.First().SeDate == null || x.Subscriptions.First().SeDate <= filter.ToDate, !(filter.ToDate is null))
                   .Where(x => x.UStatus == UStatus.InstalledActiveGprs
                                         || x.UStatus == UStatus.InstalledActiveHosting
                                         || x.UStatus == UStatus.InstalledActive);
                    break;
                }
        }
    }


}

