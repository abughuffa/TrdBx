using CleanArchitecture.Blazor.Application.Features.WialonTasks.Mappers;
using CleanArchitecture.Blazor.Application.Features.WialonTasks.Caching;
using CleanArchitecture.Blazor.Application.Features.WialonTasks.DTOs;
using CleanArchitecture.Blazor.Application.Features.WialonTasks.Specifications;
using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.WialonTasks.Queries.Pagination;

public class WialonTasksWithPaginationQuery : WialonTaskAdvancedFilter, ICacheableRequest<PaginatedData<WialonTaskDto>>
{
    public override string ToString()
    {
        return $"Search:{Keyword}, ServiceLogId: {ServiceLogId}, TrackingUnitId:{TrackingUnitId}, SortDirection:{SortDirection}, OrderBy:{OrderBy}, {PageNumber}, {PageSize}";
    }
    public string CacheKey => WialonTaskCacheKey.GetPaginationCacheKey($"{this}");
    public IEnumerable<string> Tags => WialonTaskCacheKey.Tags;
    public WialonTaskAdvancedSpecification Specification => new(this);


}

public class WialonTasksWithPaginationQueryHandler :
         IRequestHandler<WialonTasksWithPaginationQuery, PaginatedData<WialonTaskDto>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public WialonTasksWithPaginationQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public WialonTasksWithPaginationQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<PaginatedData<WialonTaskDto>> Handle(WialonTasksWithPaginationQuery request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await _context.WialonTasks.OrderBy($"{request.OrderBy} {request.SortDirection}")
        //                                         .ProjectToPaginatedDataAsync<WialonTask, WialonTaskDto>(request.Specification,
        //                                            request.PageNumber,
        //                                            request.PageSize,
        //                                            _mapper.ConfigurationProvider,
        //                                            cancellationToken);
        //return data;

        var data = await _context.WialonTasks.Include(s => s.ServiceLog).Include(s => s.TrackingUnit)
            .OrderBy($"{request.OrderBy} {request.SortDirection}")
                                                .ProjectToPaginatedDataAsync(request.Specification,
                                                                             request.PageNumber,
                                                                             request.PageSize,
                                                                             Mapper.ToDto,
                                                                             cancellationToken);
        return data;
    }
}