using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.ServiceLogs.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for advanced filtering of ServiceLogs.
/// </summary>
public class ServiceLogsSupportSpecification : Specification<ServiceLog>
{
    public ServiceLogsSupportSpecification(int[] Ids)
    {
        Query.Include(x => x.Subscriptions)
             .Where(x => Ids.Contains(x.CustomerId))
             .Where(x => x.ServiceTask == ServiceTask.Recover
                      || x.ServiceTask == ServiceTask.ReInstall
                      || x.ServiceTask == ServiceTask.Transfer
                      || x.ServiceTask == ServiceTask.InstallSimCard
                      || x.ServiceTask == ServiceTask.ReplacSimCard)
             .Where(x => x.IsDeserved.Equals(true))
             .Where(x => x.IsBilled.Equals(false));
    }
}


