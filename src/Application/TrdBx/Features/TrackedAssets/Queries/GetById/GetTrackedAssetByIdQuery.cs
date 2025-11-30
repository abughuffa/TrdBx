using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Caching;
using CleanArchitecture.Blazor.Application.Features.TrackedAssets.DTOs;
using CleanArchitecture.Blazor.Application.Features.TrackedAssets.Mappers;
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
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public GetTrackedAssetByIdQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public GetTrackedAssetByIdQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<Result<TrackedAssetDto>> Handle(GetTrackedAssetByIdQuery request, CancellationToken cancellationToken)
    {

        //await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await db.TrackedAssets.ApplySpecification(new TrackedAssetByIdSpecification(request.Id))
        //                                        .ProjectTo<TrackedAssetDto>(_mapper.ConfigurationProvider)
        //                                        .FirstAsync(cancellationToken) ?? throw new NotFoundException($"TrackedAsset with id: [{request.Id}] not found.");
        //return await Result<TrackedAssetDto>.SuccessAsync(data);

        var data = await _context.TrackedAssets.ApplySpecification(new TrackedAssetByIdSpecification(request.Id))
                           .ProjectTo()
                           .FirstAsync(cancellationToken) ?? throw new NotFoundException($"TrackedAsset with id: [{request.Id}] not found.");
        return await Result<TrackedAssetDto>.SuccessAsync(data);


    }
}
