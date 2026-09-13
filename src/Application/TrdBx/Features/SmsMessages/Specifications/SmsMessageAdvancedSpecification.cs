using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.SmsMessages.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for advanced filtering of SimCards.
/// </summary>
public class SmsMessageAdvancedSpecification : Specification<SmsMessage>
{
    public SmsMessageAdvancedSpecification(SmsMessageAdvancedFilter filter)
    {
        //var today = DateTime.UtcNow;
        //var todayrange = today.GetDateRange(SimCardListView.TODAY.ToString(), filter.LocalTimezoneOffset);
        //var last30daysrange = today.GetDateRange(SimCardListView.LAST_30_DAYS.ToString(),filter.LocalTimezoneOffset);

        Query.Where(q => q.PhoneNumber != null)
             .Where(filter.Keyword,!string.IsNullOrEmpty(filter.Keyword));
            //  .Where(filter.Keyword,!string.IsNullOrEmpty(filter.Keyword));
       
    }
}
