using CleanArchitecture.Blazor.Application.Features.ServiceLogs.Mappers;
using CleanArchitecture.Blazor.Application.Features.ServiceLogs.Caching;
using CleanArchitecture.Blazor.Application.Features.ServiceLogs.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.ServiceLogs.Queries.GetAll;

public class GetAllServiceLogsQuery : ICacheableRequest<IEnumerable<ServiceLogDto>>
{
   public string CacheKey => ServiceLogCacheKey.GetAllCacheKey;
    public IEnumerable<string> Tags => ServiceLogCacheKey.Tags;
}

public class GetAllServiceLogsQueryHandler :
     IRequestHandler<GetAllServiceLogsQuery, IEnumerable<ServiceLogDto>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public GetAllServiceLogsQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public GetAllServiceLogsQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<IEnumerable<ServiceLogDto>> Handle(GetAllServiceLogsQuery request, CancellationToken cancellationToken)
    {
        //await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await db.ServiceLogs
        //    .ProjectTo<ServiceLogDto>(_mapper.ConfigurationProvider)
        //    .ToListAsync(cancellationToken);
        //return data;

        var data = await _context.ServiceLogs.ProjectTo()
                                           .AsNoTracking()
                                           .ToListAsync(cancellationToken);
        return data;
    }
}


