using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.ServiceLogs.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for advanced filtering of ServiceLogs.
/// </summary>
public class ServiceLogsCheckSpecification : Specification<ServiceLog>
{
    public ServiceLogsCheckSpecification(int[] Ids)
    {
        Query.Where(x => Ids.Contains(x.CustomerId))
             .Where(x => x.ServiceTask.Equals(ServiceTask.Check))
             .Where(x => x.IsDeserved.Equals(true))
             .Where(x => x.IsBilled.Equals(false));
    }
}
