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

        var today = DateTime.UtcNow;
        var todayrange = today.GetDateRange(SmsMessageListView.TODAY.ToString());
        var last30daysrange = today.GetDateRange(SmsMessageListView.LAST_30_DAYS.ToString());

        Query.Where(q => q.To != null || q.From != null )
             .Where(filter.Keyword,!string.IsNullOrEmpty(filter.Keyword))
             //.Where(q => q.TenantId == filter.CurrentUser.TenantId)
             .Where(x => x.Status == filter.SmsStatus, filter.SmsStatus is not null)
             .Where(x => x.CreatedAt >= todayrange.Start && x.CreatedAt < todayrange.End.AddDays(1), filter.ListView == SmsMessageListView.TODAY)
             .Where(x => x.CreatedAt >= last30daysrange.Start, filter.ListView == SmsMessageListView.LAST_30_DAYS);
    }
}
