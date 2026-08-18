using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.WialonTasks.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for filtering WialonTasks by their ID.
/// </summary>
public class WialonTaskByIdSpecification : Specification<WialonTask>
{
    public WialonTaskByIdSpecification(int id)
    {
        Query.Where(q => q.Id == id);
    }
}