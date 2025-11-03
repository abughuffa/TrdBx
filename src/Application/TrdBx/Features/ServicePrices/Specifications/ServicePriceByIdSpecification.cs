using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.ServicePrices.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for filtering ServicePrices by their ID.
/// </summary>
public class ServicePriceByIdSpecification : Specification<ServicePrice>
{
    public ServicePriceByIdSpecification(int id)
    {
       Query.Where(q => q.Id == id);
    }
}