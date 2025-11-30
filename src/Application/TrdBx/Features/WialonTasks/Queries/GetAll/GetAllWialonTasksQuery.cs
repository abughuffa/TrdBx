using CleanArchitecture.Blazor.Application.Features.WialonTasks.Mappers;
using CleanArchitecture.Blazor.Application.Features.WialonTasks.Caching;
using CleanArchitecture.Blazor.Application.Features.WialonTasks.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.WialonTasks.Queries.GetAll;

public class GetAllWialonTasksQuery : ICacheableRequest<IEnumerable<WialonTaskDto>>
{
    public string CacheKey => WialonTaskCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => WialonTaskCacheKey.Tags;
}

public class GetAllWialonTasksQueryHandler :
     IRequestHandler<GetAllWialonTasksQuery, IEnumerable<WialonTaskDto>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public GetAllWialonTasksQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}
    private readonly IApplicationDbContext _context;
    public GetAllWialonTasksQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<IEnumerable<WialonTaskDto>> Handle(GetAllWialonTasksQuery request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await _context.WialonTasks.ProjectTo<WialonTaskDto>(_mapper.ConfigurationProvider)
        //                                        .AsNoTracking()
        //                                        .ToListAsync(cancellationToken);
        //return data;

        var data = await _context.WialonTasks.ProjectTo()
                                           .AsNoTracking()
                                           .ToListAsync(cancellationToken);
        return data;
    }
}


