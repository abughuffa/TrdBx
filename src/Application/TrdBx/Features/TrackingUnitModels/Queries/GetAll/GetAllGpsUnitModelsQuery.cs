// using CleanArchitecture.Blazor.Application.Features.TrackingUnitModels.Mappers;
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
    private readonly TypeAdapterConfig _typeAdapterConfig;
    public GetAllTrackingUnitModelsQueryHandler(
        TypeAdapterConfig typeAdapterConfig,
        IApplicationDbContextFactory dbContextFactory)
    {
        _typeAdapterConfig = typeAdapterConfig;
        _dbContextFactory = dbContextFactory;
    }
    public async ValueTask<IEnumerable<TrackingUnitModelDto>> Handle(GetAllTrackingUnitModelsQuery request, CancellationToken cancellationToken)
    {
        await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await db.TrackingUnitModels
        //    .ProjectTo<TrackingUnitModelDto>(_mapper.ConfigurationProvider)
        //    .ToListAsync(cancellationToken);
        //return data;

        var data = await _context.TrackingUnitModels.ProjectToType<TrackingUnitModelDto>(_typeAdapterConfig)
                                           .AsNoTracking()
                                           .ToListAsync(cancellationToken);
        return data;
    }
}


