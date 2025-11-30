using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.ServiceLogs.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for advanced filtering of ServiceLogs.
/// </summary>
public class ServiceLogsRenewSpecification : Specification<ServiceLog>
{
    public ServiceLogsRenewSpecification(int[] Ids)
    {
        Query.Include(x => x.Subscriptions)
             .Where(x => Ids.Contains(x.CustomerId))
             .Where(x => x.ServiceTask.Equals(ServiceTask.RenewUnitSub))
             .Where(x => x.IsDeserved.Equals(true))
             .Where(x => x.IsBilled.Equals(false));
    }
}
