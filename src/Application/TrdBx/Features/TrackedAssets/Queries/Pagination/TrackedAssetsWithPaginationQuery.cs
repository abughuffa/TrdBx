using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Caching;
using CleanArchitecture.Blazor.Application.Features.TrackedAssets.DTOs;
using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Specifications;
using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Mappers;
using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.TrackedAssets.Queries.Pagination;

public class TrackedAssetsWithPaginationQuery : TrackedAssetAdvancedFilter, ICacheableRequest<PaginatedData<TrackedAssetDto>>
{
    public override string ToString()
    {
        return $"Listview:{ListView}: Search:{Keyword}, {OrderBy}, {SortDirection}, {PageNumber}, {PageSize}";
    }

    public string CacheKey => TrackedAssetCacheKey.GetPaginationCacheKey($"{this}");
    public IEnumerable<string>? Tags => TrackedAssetCacheKey.Tags;
    public TrackedAssetAdvancedSpecification Specification => new TrackedAssetAdvancedSpecification(this);

}
    
public class TrackedAssetsWithPaginationQueryHandler :
         IRequestHandler<TrackedAssetsWithPaginationQuery, PaginatedData<TrackedAssetDto>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public TrackedAssetsWithPaginationQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}
    private readonly IApplicationDbContext _context;
    public TrackedAssetsWithPaginationQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<PaginatedData<TrackedAssetDto>> Handle(TrackedAssetsWithPaginationQuery request, CancellationToken cancellationToken)
        {
        //await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await db.TrackedAssets.OrderBy($"{request.OrderBy} {request.SortDirection}")
        //    .ProjectToPaginatedDataAsync<TrackedAsset, TrackedAssetDto>(request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);
        //return data;

        var data = await _context.TrackedAssets.Include(a=>a.TrackingUnits).OrderBy($"{request.OrderBy} {request.SortDirection}")
                                     .ProjectToPaginatedDataAsync(request.Specification,
                                                                  request.PageNumber,
                                                                  request.PageSize,
                                                                  Mapper.ToDto,
                                                                  cancellationToken);
        return data;

    }
}