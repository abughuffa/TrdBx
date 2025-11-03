using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.TrackedAssets.Specifications;
#nullable disable warnings
/// <summary>
/// Specification class for filtering TrackedAssets by their ID.
/// </summary>
public class TrackedAssetByIdSpecification : Specification<TrackedAsset>
{
    public TrackedAssetByIdSpecification(int id)
    {
       Query.Where(q => q.Id == id);
    }
}