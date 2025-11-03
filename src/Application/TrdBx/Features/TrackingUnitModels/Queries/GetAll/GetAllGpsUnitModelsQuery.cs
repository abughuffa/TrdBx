using CleanArchitecture.Blazor.Application.Features.TrackingUnitModels.Caching;
using CleanArchitecture.Blazor.Application.Features.TrackingUnitModels.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.TrackingUnitModels.Queries.GetAll;

public class GetAllTrackingUnitModelsQuery : ICacheableRequest<IEnumerable<TrackingUnitModelDto>>
{
   public string CacheKey => TrackingUnitModelCacheKey.GetAllCacheKey;
   public IEnumerable<string> Tags => TrackingUnitModelCacheKey.Tags;
}

public class GetAllTrackingUnitModelsQueryHandler :
     IRequestHandler<GetAllTrackingUnitModelsQuery, IEnumerable<TrackingUnitModelDto>>
{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IMapper _mapper;
    public GetAllTrackingUnitModelsQueryHandler(
        IApplicationDbContextFactory dbContextFactory,
        IMapper mapper
    )
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TrackingUnitModelDto>> Handle(GetAllTrackingUnitModelsQuery request, CancellationToken cancellationToken)
    {
        await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        var data = await db.TrackingUnitModels
            .ProjectTo<TrackingUnitModelDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        return data;
    }
}


