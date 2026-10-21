using CleanArchitecture.Blazor.Application.Features.WialonTasks.Caching;
using CleanArchitecture.Blazor.Application.Features.WialonTasks.DTOs;
using CleanArchitecture.Blazor.Application.Features.WialonTasks.Specifications;
// using CleanArchitecture.Blazor.Application.Features.WialonTasks.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.WialonTasks.Queries.GetByRegistredTaskId;

public class WialonTasksByServiceLogIdQuery : ICacheableRequest<IEnumerable<WialonTaskDto>>
{
    public required int Id { get; set; }
    public string CacheKey => WialonTaskCacheKey.GetByServiceLogIdCacheKey($"{Id}");
     public IEnumerable<string> Tags => WialonTaskCacheKey.Tags;
}

public class WialonTasksByServiceLogIdQueryHandler :
     IRequestHandler<WialonTasksByServiceLogIdQuery, IEnumerable<WialonTaskDto>>
{
         private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly TypeAdapterConfig _typeAdapterConfig;
    public WialonTasksByServiceLogIdQueryHandler(
        TypeAdapterConfig typeAdapterConfig,
        IApplicationDbContextFactory dbContextFactory)
    {
        _typeAdapterConfig = typeAdapterConfig;
        _dbContextFactory = dbContextFactory;
    }

    public async ValueTask<IEnumerable<WialonTaskDto>> Handle(WialonTasksByServiceLogIdQuery request, CancellationToken cancellationToken)
    {
        await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await _context.WialonTasks.ApplySpecification(new WialonTasksByServiceLogIdSpecification(request.Id))
        //                                     .ProjectTo<WialonTaskDto>(_mapper.ConfigurationProvider)
        //                                       .ToListAsync(cancellationToken);
        //return data;

        var data = await _context.WialonTasks.ApplySpecification(new WialonTasksByServiceLogIdSpecification(request.Id))
                                        .ProjectToType<WialonTaskDto>(_typeAdapterConfig)
                                        .AsNoTracking()
                                        .ToListAsync(cancellationToken);
        return data;

    }
}


