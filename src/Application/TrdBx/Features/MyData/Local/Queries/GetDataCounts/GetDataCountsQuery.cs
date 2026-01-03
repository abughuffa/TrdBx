using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.DTOs;


namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.Queries.GetDataCounts;

public class GetDataCountsQuery : IRequest<Result<DataCountDto>>
{
    //public string CacheKey => DataCountCacheKey.GetCacheKey;
    //public IEnumerable<string> Tags => DataCountCacheKey.Tags;
}

public class GetDataCountsQueryHandler :
     IRequestHandler<GetDataCountsQuery, Result<DataCountDto>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public GetDataCountsQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public GetDataCountsQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<Result<DataCountDto>> Handle(GetDataCountsQuery request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var datacount =
            new DataCountDto
            {
                SProviders = await _context.SProviders.CountAsync(),
                SPackages = await _context.SPackages.CountAsync(),
                TrackingUnitModels = await _context.TrackingUnitModels.CountAsync(),
                SimCards = await _context.SimCards.CountAsync(),
                ParentCustomer = await _context.Customers.CountAsync(x => x.ParentId == null),
                ChildCustomers = await _context.Customers.CountAsync(x => x.ParentId != null),
                TrackedAssets = await _context.TrackedAssets.CountAsync(),
                TrackingUnits = await _context.TrackingUnits.CountAsync(),
                ServiceLogs = await _context.ServiceLogs.CountAsync(),
                Subscriptions = await _context.Subscriptions.CountAsync(),
                WialonTasks = await _context.WialonTasks.CountAsync(),
            };

            return await Result<DataCountDto>.SuccessAsync(datacount);

    }
}
