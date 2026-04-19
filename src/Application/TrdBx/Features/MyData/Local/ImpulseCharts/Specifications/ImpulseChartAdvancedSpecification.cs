using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.ImpulseCharts.Specifications;
#nullable disable warnings
public class ImpulseChartAdvancedSpecification : Specification<TrackingUnit>
{



    public ImpulseChartAdvancedSpecification(ImpulseChartAdvancedFilter filter)
    {
        switch (filter.ListView)
        {
            case ImpulseChartListView.SimCardsExpiryDate:
                {
                    Query
                   .Where(x => x.CustomerId.Equals(filter.CustomerId), filter.CustomerId is not null)
                   .Where(x => x.SimCard.ExDate == null || x.SimCard.ExDate >= filter.FromDate, (filter.FromDate is not null))
                   .Where(x => x.SimCard.ExDate == null || x.SimCard.ExDate <= filter.ToDate, (filter.ToDate is not null))
                   .Where(x => x.UStatus == UStatus.InstalledActiveGprs
                                         || x.UStatus == UStatus.InstalledActiveHosting
                                         || x.UStatus == UStatus.InstalledActive);
                    break;
                }
            case ImpulseChartListView.UnitSubExpiryDate:
                {
                    Query
                   .Where(x => x.CustomerId.Equals(filter.CustomerId), filter.CustomerId is not null)
                   .Where(x => x.Subscriptions.First().SeDate == null || x.Subscriptions.First().SeDate >= filter.FromDate, (filter.FromDate is not null))
                   .Where(x => x.Subscriptions.First().SeDate == null || x.Subscriptions.First().SeDate <= filter.ToDate, (filter.ToDate is not null))
                   .Where(x => x.UStatus == UStatus.InstalledActiveGprs
                                         || x.UStatus == UStatus.InstalledActiveHosting
                                         || x.UStatus == UStatus.InstalledActive);
                    break;
                }
        }
    }


}

