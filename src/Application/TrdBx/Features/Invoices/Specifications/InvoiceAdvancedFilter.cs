using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.Invoices.Specifications;

#nullable disable warnings
/// <summary>
/// Specifies the different views available for the Invoice list.
/// </summary>
public enum InvoiceListView
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
/// A class for applying advanced filtering options to Invoice lists.
/// </summary>
public class InvoiceAdvancedFilter : PaginationFilter
{

    public int? CustomerId { get; set; } = null;

    public IStatus? IStatus { get; set; } = null;
    public InvoiceType? InvoiceType { get; set; } = null;

    public InvoiceListView ListView { get; set; } = InvoiceListView.All;
    public UserProfile? CurrentUser { get; set; }
}