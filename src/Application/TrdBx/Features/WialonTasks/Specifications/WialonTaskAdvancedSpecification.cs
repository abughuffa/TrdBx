using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.WialonTasks.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for advanced filtering of WialonTasks.
/// </summary>
public class WialonTaskAdvancedSpecification : Specification<WialonTask>
{
    public WialonTaskAdvancedSpecification(WialonTaskAdvancedFilter filter)
    {


        Query.Where(q => q.Desc != null)
             .Where(filter.Keyword, !string.IsNullOrEmpty(filter.Keyword))
             .Where(q => q.ServiceLogId == filter.ServiceLogId, !(filter.ServiceLogId.Equals(0) || filter.ServiceLogId.Equals(null)))
             .Where(q => q.TrackingUnitId == filter.TrackingUnitId, !(filter.TrackingUnitId.Equals(0) || filter.TrackingUnitId.Equals(null)));

    }
}
