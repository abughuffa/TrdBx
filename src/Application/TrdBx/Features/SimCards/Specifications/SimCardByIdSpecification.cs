using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.SimCards.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for filtering SimCards by their ID.
/// </summary>
public class SimCardByIdSpecification : Specification<SimCard>
{
    public SimCardByIdSpecification(int id)
    {
       Query.Where(q => q.Id == id);
    }
}