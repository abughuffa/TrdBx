using CleanArchitecture.Blazor.Application.Features.WialonTasks.Caching;
using CleanArchitecture.Blazor.Application.Features.WialonTasks.DTOs;
using CleanArchitecture.Blazor.Application.Features.WialonTasks.Mappers;
using CleanArchitecture.Blazor.Application.Features.WialonTasks.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.WialonTasks.Queries.GetById;

public class GetWialonTaskByIdQuery : ICacheableRequest<Result<WialonTaskDto>>
{
    public required int Id { get; set; }
    public string CacheKey => WialonTaskCacheKey.GetByIdCacheKey($"{Id}");
     public IEnumerable<string> Tags => WialonTaskCacheKey.Tags;
}

public class GetWialonTaskByIdQueryHandler :
     IRequestHandler<GetWialonTaskByIdQuery, Result<WialonTaskDto>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public GetWialonTaskByIdQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}


    private readonly IApplicationDbContext _context;
    public GetWialonTaskByIdQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<Result<WialonTaskDto>> Handle(GetWialonTaskByIdQuery request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await _context.WialonTasks.ApplySpecification(new WialonTaskByIdSpecification(request.Id))
        //                                       .ProjectTo<WialonTaskDto>(_mapper.ConfigurationProvider)
        //                                        .FirstAsync(cancellationToken);
        //return await Result<WialonTaskDto>.SuccessAsync(data);

        var data = await _context.WialonTasks.ApplySpecification(new WialonTaskByIdSpecification(request.Id))
                                        .ProjectTo()
                                        .FirstAsync(cancellationToken);
        return await Result<WialonTaskDto>.SuccessAsync(data);
    }
}
