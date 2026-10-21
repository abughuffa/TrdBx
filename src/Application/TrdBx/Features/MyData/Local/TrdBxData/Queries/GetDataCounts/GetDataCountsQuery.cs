using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.TrdBxData.DTOs;


namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Local.TrdBxData.Queries.GetDataCounts;

public class GetDataCountsQuery : IRequest<Result<DataCountDto>>
{
    //public string CacheKey => DataCountCacheKey.GetCacheKey;
    //public IEnumerable<string> Tags => DataCountCacheKey.Tags;
}

public class GetDataCountsQueryHandler :
     IRequestHandler<GetDataCountsQuery, Result<DataCountDto>>
{

        private readonly IApplicationDbContextFactory _dbContextFactory;

        public GetDataCountsQueryHandler(

            IApplicationDbContextFactory dbContextFactory

            )
        {

            _dbContextFactory = dbContextFactory;

        }

    public async ValueTask<Result<DataCountDto>> Handle(GetDataCountsQuery request, CancellationToken cancellationToken)
    {
        await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        var datacount =
            new DataCountDto
            {
                SProviders = _context.SProviders.Count(),
                SPackages =  _context.SPackages.Count(),
                TrackingUnitModels =  _context.TrackingUnitModels.Count(),
                SimCards =  _context.SimCards.Count(),
                ParentCustomer =  _context.Customers.Count(x => x.ParentId == null),
                ChildCustomers =  _context.Customers.Count(x => x.ParentId != null),
                TrackedAssets =  _context.TrackedAssets.Count(),
                TrackingUnits =  _context.TrackingUnits.Count(),
                ServiceLogs =  _context.ServiceLogs.Count(),
                Subscriptions =  _context.Subscriptions.Count(),
                WialonTasks =  _context.WialonTasks.Count(),
            };

            return await Result<DataCountDto>.SuccessAsync(datacount);

    }
}
