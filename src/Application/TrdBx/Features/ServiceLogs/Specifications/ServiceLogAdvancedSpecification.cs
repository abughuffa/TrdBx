using CleanArchitecture.Blazor.Application.Features.ServiceLogs.Specifications;
using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.ServiceLogs.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for advanced filtering of ServiceLogs.
/// </summary>
public class ServiceLogAdvancedSpecification : Specification<ServiceLog>
{
    public ServiceLogAdvancedSpecification(ServiceLogAdvancedFilter filter)
    {
        var today = DateTime.UtcNow;
        var todayrange = today.GetDateRange(ServiceLogListView.TODAY.ToString());


        Query.Where(q => q.Subscriptions.Any(s => s.TrackingUnitId == filter.TrackingUnitId) ||
                         q.WialonTasks.Any(w => w.TrackingUnitId == filter.TrackingUnitId), !(filter.TrackingUnitId.Equals(0) || filter.TrackingUnitId.Equals(null)))
            .Where(x => x.ServiceTask == filter.ServiceTask, !filter.ServiceTask.Equals(ServiceTask.All))
            .Where(x => x.IsBilled == filter.IsBilled, !filter.IsBilled.Equals(null))
            .Where(x => x.Created >= todayrange.Start && x.Created < todayrange.End.AddDays(1), filter.ListView == ServiceLogListView.TODAY);

    }
}
