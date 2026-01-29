using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.ServiceLogs.Specifications;



/// <summary>
/// A class for applying advanced filtering options to ServiceLog lists.
/// </summary>
public class ServiceLogAdvancedFilter : PaginationFilter
{
    public int? TrackingUnitId { get; set; } = null;
    public int? CustomerId { get; set; } = null;
    public bool? IsBilled { get; set; } = null;
    public ServiceTask? ServiceTask { get; set; } = null;
    public ServiceLogListView ListView { get; set; } = ServiceLogListView.All;


}