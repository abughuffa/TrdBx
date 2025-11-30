using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.Tickets.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for advanced filtering of Tickets.
/// </summary>
public class TicketAdvancedSpecification : Specification<Ticket>
{
    public TicketAdvancedSpecification(TicketAdvancedFilter filter)
    {
        var today = DateTime.UtcNow;
        var todayrange = today.GetDateRange(TicketListView.TODAY.ToString());
        var last30daysrange = today.GetDateRange(TicketListView.LAST_30_DAYS.ToString());


        Query.Where(q => q.TicketNo != null)
             .Where(filter.Keyword,!string.IsNullOrEmpty(filter.Keyword))
             .Where(x => x.ServiceTask == filter.ServiceTask, filter.ServiceTask != ServiceTask.All)
             .Where(x => x.TicketStatus == filter.TicketStatus, filter.TicketStatus != TicketStatus.All)
             .Where(q => q.CreatedBy == filter.CurrentUser.UserId, filter.ListView == TicketListView.My && filter.CurrentUser is not null)
             .Where(x => x.Created >= todayrange.Start && x.Created < todayrange.End.AddDays(1), filter.ListView == TicketListView.TODAY)
             .Where(x => x.Created >= last30daysrange.Start, filter.ListView == TicketListView.LAST_30_DAYS);
       
    }
}
