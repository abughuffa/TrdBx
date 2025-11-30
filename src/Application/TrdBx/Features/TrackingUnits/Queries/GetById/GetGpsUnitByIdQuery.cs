using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.DTOs;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Mappers;
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
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public GetTrackingUnitByIdQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}
    private readonly IApplicationDbContext _context;
    public GetTrackingUnitByIdQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<Result<TrackingUnitDto>> Handle(GetTrackingUnitByIdQuery request, CancellationToken cancellationToken)
    {
        //await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await db.TrackingUnits.ApplySpecification(new TrackingUnitByIdSpecification(request.Id))
        //                                        .ProjectTo<TrackingUnitDto>(_mapper.ConfigurationProvider)
        //                                        .FirstAsync(cancellationToken) ?? throw new NotFoundException($"TrackingUnit with id: [{request.Id}] not found.");
        //return await Result<TrackingUnitDto>.SuccessAsync(data);


        var data = await _context.TrackingUnits.ApplySpecification(new TrackingUnitByIdSpecification(request.Id))
                           .ProjectTo()
                           .FirstAsync(cancellationToken) ?? throw new NotFoundException($"TrackingUnit with id: [{request.Id}] not found.");
        return await Result<TrackingUnitDto>.SuccessAsync(data);
    }
}
