using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.WialonTasks.Specifications;

/// <summary>
/// Specification class for filtering WialonTasks by their ID.
/// </summary>
public class WialonTasksByServiceLogIdSpecification : Specification<WialonTask>
{
    public WialonTasksByServiceLogIdSpecification(int id)
    {
        Query.Where(q => q.ServiceLogId == id);
    }
}