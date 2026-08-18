// using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Mappers;
using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Caching;
using CleanArchitecture.Blazor.Application.Features.TrackedAssets.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.TrackedAssets.Queries.GetAll;

public class GetAllTrackedAssetsQuery : ICacheableRequest<IEnumerable<TrackedAssetDto>>
{
   public string CacheKey => TrackedAssetCacheKey.GetAllCacheKey;
    public IEnumerable<string> Tags => TrackedAssetCacheKey.Tags;
}

public class GetAllTrackedAssetsQueryHandler :
     IRequestHandler<GetAllTrackedAssetsQuery, IEnumerable<TrackedAssetDto>>
{
        private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly TypeAdapterConfig _typeAdapterConfig;
    public GetAllTrackedAssetsQueryHandler(
        TypeAdapterConfig typeAdapterConfig,
        IApplicationDbContextFactory dbContextFactory)
    {
        _typeAdapterConfig = typeAdapterConfig;
        _dbContextFactory = dbContextFactory;
    }
    public async ValueTask<IEnumerable<TrackedAssetDto>> Handle(GetAllTrackedAssetsQuery request, CancellationToken cancellationToken)
    {
        await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await db.TrackedAssets
        //    .ProjectTo<TrackedAssetDto>(_mapper.ConfigurationProvider)
        //    .ToListAsync(cancellationToken);
        //return data;

        var data = await _context.TrackedAssets.ProjectToType<TrackedAssetDto>(_typeAdapterConfig)
                                           .AsNoTracking()
                                           .ToListAsync(cancellationToken);
        return data;
    }
}


