using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.DTOs;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Specifications;
using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Mappers;
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
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public GetAvaliableTrackingUnitsQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public GetAvaliableTrackingUnitsQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TrackingUnitDto>> Handle(GetAvaliableTrackingUnitsQuery request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var cc = await _context.Customers.Where(cc => cc.Id == request.Id).FirstAsync(cancellationToken);
        if (cc.BillingPlan == BillingPlan.Advanced)
        {
            var c = await _context.Customers.Where(c => c.Id == cc.ParentId).ToListAsync(cancellationToken);
            var Ids = c.Select(obj => obj.Id).ToArray();

            var data = await _context.TrackingUnits.Include(u=>u.Subscriptions).ThenInclude(s=>s.ServiceLog).ApplySpecification(new AvaliableTrackingUnitsSpecification(Ids))
                                        .ProjectTo()
                                        .ToListAsync(cancellationToken);
            return data;
        }
        else
        {
            var data = await _context.TrackingUnits.Include(u => u.Subscriptions).ThenInclude(s => s.ServiceLog).ApplySpecification(new AvaliableTrackingUnitsSpecification(new int[] { (int)request.Id }))
                                        .ProjectTo()
                                        .ToListAsync(cancellationToken);
            return data;
        }



    }
}
