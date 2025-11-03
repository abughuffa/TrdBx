using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.DTOs;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Specifications;
using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Queries.GetAvaliable;

public class GetAvaliableTrackingUnitsQuery : ICacheableRequest<IEnumerable<TrackingUnitDto>>
{
    public int? Id { get; set; }
    public string CacheKey => TrackingUnitCacheKey.GetAvaliableCacheKey;
   public IEnumerable<string> Tags => TrackingUnitCacheKey.Tags;
}

public class GetAvaliableTrackingUnitsQueryHandler :
     IRequestHandler<GetAvaliableTrackingUnitsQuery, IEnumerable<TrackingUnitDto>>
{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IMapper _mapper;
    public GetAvaliableTrackingUnitsQueryHandler(
        IApplicationDbContextFactory dbContextFactory,
        IMapper mapper
    )
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TrackingUnitDto>> Handle(GetAvaliableTrackingUnitsQuery request, CancellationToken cancellationToken)
    {
        await using var db = await _dbContextFactory.CreateAsync(cancellationToken);

        var cc = await db.Customers.Where(cc => cc.Id == request.Id).FirstAsync(cancellationToken);
        if (cc.BillingPlan == BillingPlan.Advanced)
        {
            var c = await db.Customers.Where(c => c.Id == cc.ParentId).ToListAsync(cancellationToken);
            var Ids = c.Select(obj => obj.Id).ToArray();

            var data = await db.TrackingUnits.ApplySpecification(new AvaliableTrackingUnitsSpecification(Ids))
                                        .ProjectTo<TrackingUnitDto>(_mapper.ConfigurationProvider)
                                        .ToListAsync(cancellationToken);
            return data;
        }
        else
        {
            var data = await db.TrackingUnits.ApplySpecification(new AvaliableTrackingUnitsSpecification(new int[] { (int)request.Id }))
                                        .ProjectTo<TrackingUnitDto>(_mapper.ConfigurationProvider)
                                        .ToListAsync(cancellationToken);
            return data;
        }



    }
}
