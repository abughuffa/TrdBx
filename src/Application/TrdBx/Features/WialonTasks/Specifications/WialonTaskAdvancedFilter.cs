namespace CleanArchitecture.Blazor.Application.Features.WialonTasks.Specifications;


public class WialonTaskAdvancedFilter : PaginationFilter
{
    public int? TrackingUnitId { get; set; }
    public int? ServiceLogId { get; set; }
}