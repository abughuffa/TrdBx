using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.TrackedAssets.Specifications;

/// <summary>
/// Specification class for filtering TrackedAssets by their ID.
/// </summary>
public class AvaliableTrackedAssetsSpecification : Specification<TrackedAsset>
{
    public AvaliableTrackedAssetsSpecification()
    {
       Query.Where(q => q.IsAvaliable == true);
    }
}