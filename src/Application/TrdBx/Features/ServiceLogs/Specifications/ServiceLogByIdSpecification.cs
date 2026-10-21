using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.ServiceLogs.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for filtering ServiceLogs by their ID.
/// </summary>
public class ServiceLogByIdSpecification : Specification<ServiceLog>
{
    public ServiceLogByIdSpecification(int id)
    {
       Query.Where(q => q.Id == id);
    }
}