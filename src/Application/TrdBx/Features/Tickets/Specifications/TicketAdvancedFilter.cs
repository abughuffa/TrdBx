using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.Tickets.Specifications;

#nullable disable warnings
/// <summary>
/// Specifies the different views available for the Ticket list.
/// </summary>
public enum TicketListView
{
    [Description("All")]
    All,
    [Description("My")]
    My,
    [Description("Created Toady")]
    TODAY,
    [Description("Created within the last 30 days")]
    LAST_30_DAYS
}
/// <summary>
/// A class for applying advanced filtering options to Ticket lists.
/// </summary>
public class TicketAdvancedFilter: PaginationFilter
{
    public ServiceTask? ServiceTask { get; set; } = null;
    public TicketStatus? TicketStatus { get; set; } = null;
    public TicketListView ListView { get; set; } = TicketListView.All;
    public UserProfile? CurrentUser { get; set; }
}