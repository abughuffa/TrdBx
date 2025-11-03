using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Caching;
using CleanArchitecture.Blazor.Application.Features.TrackedAssets.DTOs;
using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.TrackedAssets.Queries.GetAvaliable;

public class GetAvaliableTrackedAssetsQuery : ICacheableRequest<IEnumerable<TrackedAssetDto>>
{

   public string CacheKey => TrackedAssetCacheKey.GetAvaliableCacheKey;
   public IEnumerable<string> Tags => TrackedAssetCacheKey.Tags;
}

public class GetAvaliableTrackedAssetsQueryHandler :
     IRequestHandler<GetAvaliableTrackedAssetsQuery, IEnumerable<TrackedAssetDto>>
{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IMapper _mapper;
    public GetAvaliableTrackedAssetsQueryHandler(
        IApplicationDbContextFactory dbContextFactory,
        IMapper mapper
    )
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TrackedAssetDto>> Handle(GetAvaliableTrackedAssetsQuery request, CancellationToken cancellationToken)
    {
        await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        var data = await db.TrackedAssets.ApplySpecification(new AvaliableTrackedAssetsSpecification())
            .ProjectTo<TrackedAssetDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        return data;

    }
}
