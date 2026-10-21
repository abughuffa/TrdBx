// using CleanArchitecture.Blazor.Application.Features.WialonTasks.Mappers;
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
       private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly TypeAdapterConfig _typeAdapterConfig;
    public GetAllWialonTasksQueryHandler(
        TypeAdapterConfig typeAdapterConfig,
        IApplicationDbContextFactory dbContextFactory)
    {
        _typeAdapterConfig = typeAdapterConfig;
        _dbContextFactory = dbContextFactory;
    }
    public async ValueTask<IEnumerable<WialonTaskDto>> Handle(GetAllWialonTasksQuery request, CancellationToken cancellationToken)
    {
        await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await _context.WialonTasks.ProjectTo<WialonTaskDto>(_mapper.ConfigurationProvider)
        //                                        .AsNoTracking()
        //                                        .ToListAsync(cancellationToken);
        //return data;

        var data = await _context.WialonTasks.ProjectToType<WialonTaskDto>(_typeAdapterConfig)
                                           .AsNoTracking()
                                           .ToListAsync(cancellationToken);
        return data;
    }
}


