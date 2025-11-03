using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.ServiceLogs.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for advanced filtering of ServiceLogs.
/// </summary>
public class ServiceLogsSubscriptionSpecification : Specification<ServiceLog>
{
    public ServiceLogsSubscriptionSpecification(int[] Ids)
    {
        Query.Include(x => x.Subscriptions)

             .Where(x => Ids.Contains(x.CustomerId))
             .Where(x => x.ServiceTask == ServiceTask.ActivateUnit 
                      || x.ServiceTask == ServiceTask.ActivateUnitForGprs 
                      || x.ServiceTask == ServiceTask.ActivateUnitForHosting 
                      || x.ServiceTask == ServiceTask.DeactivateUnit)
             .Where(x => x.IsDeserved.Equals(true))
             .Where(x => x.IsBilled.Equals(false));
    }
}
