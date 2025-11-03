using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.SimCards.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for filtering SimCards by their ID.
/// </summary>
public class AvaliableSimCardSpecification : Specification<SimCard>
{
    public AvaliableSimCardSpecification(int[]? Ids)
    {
        Query.Where(q => q.SStatus == SStatus.New || q.SStatus == SStatus.Used || Ids.Contains(q.Id));
    }
}