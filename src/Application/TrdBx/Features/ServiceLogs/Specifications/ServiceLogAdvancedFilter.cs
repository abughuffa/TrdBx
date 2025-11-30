using CleanArchitecture.Blazor.Application.Features.ServiceLogs.Specifications;
using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.ServiceLogs.Specifications;



/// <summary>
/// A class for applying advanced filtering options to ServiceLog lists.
/// </summary>
public class ServiceLogAdvancedFilter : PaginationFilter
{
    public int TrackingUnitId { get; set; } = 0;
    public int CustomerId { get; set; } = 0;
    public bool? IsBilled { get; set; } = null;
    public ServiceTask ServiceTask { get; set; } = ServiceTask.All;
    public ServiceLogListView ListView { get; set; } = ServiceLogListView.All;


}