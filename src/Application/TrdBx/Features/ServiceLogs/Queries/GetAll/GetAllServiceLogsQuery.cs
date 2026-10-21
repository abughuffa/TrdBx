// using CleanArchitecture.Blazor.Application.Features.ServiceLogs.Mappers;
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
       private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly TypeAdapterConfig _typeAdapterConfig;
    public GetAllServiceLogsQueryHandler(
        TypeAdapterConfig typeAdapterConfig,
        IApplicationDbContextFactory dbContextFactory)
    {
        _typeAdapterConfig = typeAdapterConfig;
        _dbContextFactory = dbContextFactory;
    }

    public async ValueTask<IEnumerable<ServiceLogDto>> Handle(GetAllServiceLogsQuery request, CancellationToken cancellationToken)
    {
        await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await db.ServiceLogs
        //    .ProjectTo<ServiceLogDto>(_mapper.ConfigurationProvider)
        //    .ToListAsync(cancellationToken);
        //return data;

        var data = await _context.ServiceLogs.ProjectToType<ServiceLogDto>(_typeAdapterConfig)
                                           .AsNoTracking()
                                           .ToListAsync(cancellationToken);
        return data;
    }
}


