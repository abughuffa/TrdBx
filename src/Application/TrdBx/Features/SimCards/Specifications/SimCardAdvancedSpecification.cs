using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.SimCards.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for advanced filtering of SimCards.
/// </summary>
public class SimCardAdvancedSpecification : Specification<SimCard>
{
    public SimCardAdvancedSpecification(SimCardAdvancedFilter filter)
    {
        //var today = DateTime.UtcNow;
        //var todayrange = today.GetDateRange(SimCardListView.TODAY.ToString(), filter.LocalTimezoneOffset);
        //var last30daysrange = today.GetDateRange(SimCardListView.LAST_30_DAYS.ToString(),filter.LocalTimezoneOffset);

        Query.Where(q => q.SimCardNo != null)
             .Where(filter.Keyword,!string.IsNullOrEmpty(filter.Keyword));
       
    }
}
