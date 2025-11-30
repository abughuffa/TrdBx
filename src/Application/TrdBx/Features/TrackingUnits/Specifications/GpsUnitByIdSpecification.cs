using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for filtering TrackingUnits by their ID.
/// </summary>
public class TrackingUnitByIdSpecification : Specification<TrackingUnit>
{
    public TrackingUnitByIdSpecification(int id)
    {
       Query.Where(q => q.Id == id);
    }
}