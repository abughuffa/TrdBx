using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Caching;
using CleanArchitecture.Blazor.Application.Features.TrackedAssets.DTOs;
// using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Mappers;
using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.TrackedAssets.Queries.GetById;

public class GetTrackedAssetByIdQuery : ICacheableRequest<Result<TrackedAssetDto>>
{
    public required int Id { get; set; }
    public string CacheKey => TrackedAssetCacheKey.GetByIdCacheKey($"{Id}");
    public IEnumerable<string> Tags => TrackedAssetCacheKey.Tags;

}

public class GetTrackedAssetByIdQueryHandler :
     IRequestHandler<GetTrackedAssetByIdQuery, Result<TrackedAssetDto>>
{
           private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly TypeAdapterConfig _typeAdapterConfig;
    public GetTrackedAssetByIdQueryHandler(
        TypeAdapterConfig typeAdapterConfig,
        IApplicationDbContextFactory dbContextFactory)
    {
        _typeAdapterConfig = typeAdapterConfig;
        _dbContextFactory = dbContextFactory;
    }
    public async ValueTask<Result<TrackedAssetDto>> Handle(GetTrackedAssetByIdQuery request, CancellationToken cancellationToken)
    {

        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        var data = await context.TrackedAssets.ApplySpecification(new TrackedAssetByIdSpecification(request.Id))
                         .ProjectToType<TrackedAssetDto>(_typeAdapterConfig)
                           .FirstAsync(cancellationToken) ?? throw new NotFoundException($"TrackedAsset with id: [{request.Id}] not found.");
        return await Result<TrackedAssetDto>.SuccessAsync(data);


    }
}
