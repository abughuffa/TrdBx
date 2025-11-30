using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for filtering TrackingUnits by their ID.
/// </summary>
public class AvaliableTrackingUnitsSpecification : Specification<TrackingUnit>
{
    public AvaliableTrackingUnitsSpecification(int[] Ids)
    {
        if (Ids.Count() == 0)
        {
            Query.Where(q => q.UStatus == UStatus.New);
        }
        else
        {
            Query.Where(q => q.UStatus == UStatus.New ||
     q.UStatus == UStatus.Reserved && Ids.Contains((int)q.CustomerId) ||
     q.UStatus == UStatus.Used && Ids.Contains((int)q.CustomerId));
        }


    }
}