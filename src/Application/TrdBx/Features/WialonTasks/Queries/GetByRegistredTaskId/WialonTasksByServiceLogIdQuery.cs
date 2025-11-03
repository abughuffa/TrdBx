using CleanArchitecture.Blazor.Application.Features.WialonTasks.Caching;
using CleanArchitecture.Blazor.Application.Features.WialonTasks.DTOs;
using CleanArchitecture.Blazor.Application.Features.WialonTasks.Specifications;

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
    private readonly IMapper _mapper;
    public WialonTasksByServiceLogIdQueryHandler(
        IApplicationDbContextFactory dbContextFactory,
        IMapper mapper
    )
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
    }

    public async Task<IEnumerable<WialonTaskDto>> Handle(WialonTasksByServiceLogIdQuery request, CancellationToken cancellationToken)
    {
        await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        var data = await _context.WialonTasks.ApplySpecification(new WialonTasksByServiceLogIdSpecification(request.Id))
                                             .ProjectTo<WialonTaskDto>(_mapper.ConfigurationProvider)
                                               .ToListAsync(cancellationToken);
        return data;

    }
}


