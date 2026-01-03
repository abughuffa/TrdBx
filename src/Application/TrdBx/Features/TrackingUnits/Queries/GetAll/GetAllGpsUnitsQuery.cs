//using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Mappers;
//using CleanArchitecture.Blazor.Application.Features.TrackingUnits.Caching;
//using CleanArchitecture.Blazor.Application.Features.TrackingUnits.DTOs;

//namespace CleanArchitecture.Blazor.Application.Features.TrackingUnits.Queries.GetAll;

//public class GetAllTrackingUnitsQuery : ICacheableRequest<IEnumerable<TrackingUnitDto>>
//{
//   public string CacheKey => TrackingUnitCacheKey.GetAllCacheKey;
//   public IEnumerable<string> Tags => TrackingUnitCacheKey.Tags;
//}

//public class GetAllTrackingUnitsQueryHandler :
//     IRequestHandler<GetAllTrackingUnitsQuery, IEnumerable<TrackingUnitDto>>
//{
//    //private readonly IApplicationDbContextFactory _dbContextFactory;
//    //private readonly IMapper _mapper;
//    //public GetAllTrackingUnitsQueryHandler(
//    //    IApplicationDbContextFactory dbContextFactory,
//    //    IMapper mapper
//    //)
//    //{
//    //    _dbContextFactory = dbContextFactory;
//    //    _mapper = mapper;
//    //}
//    private readonly IApplicationDbContext _context;
//    public GetAllTrackingUnitsQueryHandler(
//        IApplicationDbContext context
//    )
//    {
//        _context = context;
//    }

//    public async Task<IEnumerable<TrackingUnitDto>> Handle(GetAllTrackingUnitsQuery request, CancellationToken cancellationToken)
//    {
//        ////await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
//        ////var data = await db.TrackingUnits
//        ////    .ProjectTo<TrackingUnitDto>(_mapper.ConfigurationProvider)
//        ////    .ToListAsync(cancellationToken);
//        ////return data;
//        var data = await _context.TrackingUnits.ProjectTo()
//                                           .AsNoTracking()
//                                           .ToListAsync(cancellationToken);
//        return data;
//    }
//}


