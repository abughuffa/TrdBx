using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Mappers;
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
    ////private readonly IApplicationDbContextFactory _dbContextFactory;
    ////private readonly IMapper _mapper;
    ////public GetAllTrackedAssetsQueryHandler(
    ////    IApplicationDbContextFactory dbContextFactory,
    ////    IMapper mapper
    ////)
    ////{
    ////    _dbContextFactory = dbContextFactory;
    ////    _mapper = mapper;
    ////}
    private readonly IApplicationDbContext _context;
    public GetAllTrackedAssetsQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<IEnumerable<TrackedAssetDto>> Handle(GetAllTrackedAssetsQuery request, CancellationToken cancellationToken)
    {
        //await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await db.TrackedAssets
        //    .ProjectTo<TrackedAssetDto>(_mapper.ConfigurationProvider)
        //    .ToListAsync(cancellationToken);
        //return data;

        var data = await _context.TrackedAssets.ProjectTo()
                                           .AsNoTracking()
                                           .ToListAsync(cancellationToken);
        return data;
    }
}


