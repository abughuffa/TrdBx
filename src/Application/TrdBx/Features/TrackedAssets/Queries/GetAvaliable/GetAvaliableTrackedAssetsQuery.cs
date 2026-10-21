using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Caching;
using CleanArchitecture.Blazor.Application.Features.TrackedAssets.DTOs;
using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Specifications;
// using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.TrackedAssets.Queries.GetAvaliable;

public class GetAvaliableTrackedAssetsQuery : ICacheableRequest<IEnumerable<TrackedAssetDto>>
{

   public int? Id { get; set; }
   public string CacheKey => TrackedAssetCacheKey.GetAvaliableTrackedAssetsWithIdCacheKey($"{Id}");

   public IEnumerable<string> Tags => TrackedAssetCacheKey.Tags;
}

public class GetAvaliableTrackedAssetsQueryHandler :
     IRequestHandler<GetAvaliableTrackedAssetsQuery, IEnumerable<TrackedAssetDto>>
{
     private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly TypeAdapterConfig _typeAdapterConfig;
    public GetAvaliableTrackedAssetsQueryHandler(
        TypeAdapterConfig typeAdapterConfig,
        IApplicationDbContextFactory dbContextFactory)
    {
        _typeAdapterConfig = typeAdapterConfig;
        _dbContextFactory = dbContextFactory;
    }

    public async ValueTask<IEnumerable<TrackedAssetDto>> Handle(GetAvaliableTrackedAssetsQuery request, CancellationToken cancellationToken)
    {
        await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

         int? id = request.Id is null ? -1 : request.Id;

         var data = await _context.TrackedAssets
            .Where(q => q.IsAvailable || q.Id == id)
            .ProjectToType<TrackedAssetDto>(_typeAdapterConfig).ToListAsync(cancellationToken);
            
        return data;

    }
}
