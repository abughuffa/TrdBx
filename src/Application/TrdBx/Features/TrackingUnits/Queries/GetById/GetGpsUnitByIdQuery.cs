using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.DTOs;
// using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Mappers;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Queries.GetById;

public class GetTrackingUnitByIdQuery : ICacheableRequest<Result<TrackingUnitDto>>
{
   public required int Id { get; set; }
   public string CacheKey => TrackingUnitCacheKey.GetByIdCacheKey($"{Id}");
   public IEnumerable<string> Tags => TrackingUnitCacheKey.Tags;
}

public class GetTrackingUnitByIdQueryHandler :
     IRequestHandler<GetTrackingUnitByIdQuery, Result<TrackingUnitDto>>
{
            private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly TypeAdapterConfig _typeAdapterConfig;
    public GetTrackingUnitByIdQueryHandler(
        TypeAdapterConfig typeAdapterConfig,
        IApplicationDbContextFactory dbContextFactory)
    {
        _typeAdapterConfig = typeAdapterConfig;
        _dbContextFactory = dbContextFactory;
    }

    public async ValueTask<Result<TrackingUnitDto>> Handle(GetTrackingUnitByIdQuery request, CancellationToken cancellationToken)
    {
       await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        var data = await context.TrackingUnits.ApplySpecification(new TrackingUnitByIdSpecification(request.Id))
                           .ProjectToType<TrackingUnitDto>(_typeAdapterConfig)
                           .FirstAsync(cancellationToken) ?? throw new NotFoundException($"TrackingUnit with id: [{request.Id}] not found.");
        return await Result<TrackingUnitDto>.SuccessAsync(data);
    }
}
