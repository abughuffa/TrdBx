using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.ServicePrices.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for advanced filtering of ServicePrices.
/// </summary>
public class ServicePriceAdvancedSpecification : Specification<ServicePrice>
{
    public ServicePriceAdvancedSpecification()
    {
        //var today = DateTime.UtcNow;
        //var todayrange = today.GetDateRange(ServicePriceListView.TODAY.ToString(), filter.LocalTimezoneOffset);
        //var last30daysrange = today.GetDateRange(ServicePriceListView.LAST_30_DAYS.ToString(),filter.LocalTimezoneOffset);

        //Query.Where(q => q.Desc != null)
        //     .Where(filter.Keyword, !string.IsNullOrEmpty(filter.Keyword));

    }
}
