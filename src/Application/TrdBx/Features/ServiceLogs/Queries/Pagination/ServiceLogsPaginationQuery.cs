using CleanArchitecture.Blazor.Application.Features.ServiceLogs.Caching;
using CleanArchitecture.Blazor.Application.Features.ServiceLogs.DTOs;
using CleanArchitecture.Blazor.Application.Features.ServiceLogs.Mappers;
using CleanArchitecture.Blazor.Application.Features.ServiceLogs.Specifications;


namespace CleanArchitecture.Blazor.Application.Features.ServiceLogs.Queries.Pagination;

public class ServiceLogsWithPaginationQuery : ServiceLogAdvancedFilter, ICacheableRequest<PaginatedData<ServiceLogDto>>
{
    public override string ToString()
    {
        return
            $"ListView:{ListView}, Search:{Keyword}, TrackingUnitId:{TrackingUnitId}, IsBilled: {IsBilled}, Client/Customer: {CustomerId}, ServiceTask: {ServiceTask}, SortDirection: {SortDirection}, OrderBy: {OrderBy}, {PageNumber}, {PageSize}";
    }
    public string CacheKey => ServiceLogCacheKey.GetPaginationCacheKey($"{this}");
    public IEnumerable<string> Tags => ServiceLogCacheKey.Tags;
    public ServiceLogAdvancedSpecification Specification => new ServiceLogAdvancedSpecification(this);
}

public class ServiceLogsWithPaginationQueryHandler :
         IRequestHandler<ServiceLogsWithPaginationQuery, PaginatedData<ServiceLogDto>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public ServiceLogsWithPaginationQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}
    private readonly IApplicationDbContext _context;
    public ServiceLogsWithPaginationQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<PaginatedData<ServiceLogDto>> Handle(ServiceLogsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);

        if ((request.CustomerId.Equals(0) || request.CustomerId.Equals(null)))
        {
            //var data = await _context.ServiceLogs.Include(s => s.Subscriptions).Include(s => s.WialonTasks).OrderBy($"{request.OrderBy} {request.SortDirection}")
            //.ProjectToPaginatedDataAsync<ServiceLog, ServiceLogDto>(request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);
            var data = await _context.ServiceLogs.Include(s => s.Subscriptions).Include(s => s.WialonTasks).OrderBy($"{request.OrderBy} {request.SortDirection}")
                                               .ProjectToPaginatedDataAsync(request.Specification,request.PageNumber,request.PageSize,Mapper.ToDto,cancellationToken);
            return data;
        }
        else
        {
            var customer = await _context.Customers.FindAsync(request.CustomerId);

            if (customer.ParentId is null)
            {
                //Get for Parent Customer
                int[] customerIds = await _context.Customers.Where(c => c.ParentId == request.CustomerId).Select(c => c.Id).ToArrayAsync(cancellationToken);
                //var data = await _context.ServiceLogs.Where(x => customerIds.Contains(x.CustomerId))
                    //.Include(s => s.Subscriptions).Include(s => s.WialonTasks).OrderBy($"{request.OrderBy} {request.SortDirection}")
                  //.ProjectToPaginatedDataAsync<ServiceLog, ServiceLogDto>(request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);
                var data = await _context.ServiceLogs.Where(x => customerIds.Contains(x.CustomerId))
                    .Include(s => s.Subscriptions).Include(s => s.WialonTasks).OrderBy($"{request.OrderBy} {request.SortDirection}")
                                   .ProjectToPaginatedDataAsync(request.Specification, request.PageNumber, request.PageSize, Mapper.ToDto, cancellationToken);
                return data;
            }
            else
            {
                //Get for Child Customer
                //var data = await _context.ServiceLogs.Where(x => x.CustomerId.Equals(request.CustomerId))
                //.Include(s => s.Subscriptions).Include(s => s.WialonTasks).OrderBy($"{request.OrderBy} {request.SortDirection}")
                //.ProjectToPaginatedDataAsync<ServiceLog, ServiceLogDto>(request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);
                var data = await _context.ServiceLogs.Where(x => x.CustomerId.Equals(request.CustomerId))
    .Include(s => s.Subscriptions).Include(s => s.WialonTasks).OrderBy($"{request.OrderBy} {request.SortDirection}")
                   .ProjectToPaginatedDataAsync(request.Specification, request.PageNumber, request.PageSize, Mapper.ToDto, cancellationToken);
                return data;
            }
        }

    }
}











 