using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.CusPrices.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for filtering CusPrices by their ID.
/// </summary>
public class CusPriceByIdSpecification : Specification<CusPrice>
{
    public CusPriceByIdSpecification(int id)
    {
        Query.Where(q => q.Id == id);
    }
}