using CleanArchitecture.Blazor.Application.Features.WialonUnits.Caching;
using CleanArchitecture.Blazor.Application.Features.WialonUnits.DTOs;
using CleanArchitecture.Blazor.Application.Features.WialonUnits.Specifications;


namespace CleanArchitecture.Blazor.Application.Features.WialonUnits.Queries.GetById;

public class GetWialonUnitByIdQuery : ICacheableRequest<Result<WialonUnitDto>>
{
    public required int Id { get; set; }
    public string CacheKey => WialonUnitCacheKey.GetByIdCacheKey($"{Id}");
     public IEnumerable<string> Tags => WialonUnitCacheKey.Tags;
}

public class GetWialonUnitByIdQueryHandler :
     IRequestHandler<GetWialonUnitByIdQuery, Result<WialonUnitDto>>
{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IMapper _mapper;
    public GetWialonUnitByIdQueryHandler(
        IApplicationDbContextFactory dbContextFactory,
        IMapper mapper
    )
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
    }

    public async Task<Result<WialonUnitDto>> Handle(GetWialonUnitByIdQuery request, CancellationToken cancellationToken)
    {
        await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        var data = await _context.WialonUnits.ApplySpecification(new WialonUnitByIdSpecification(request.Id))
                                              .ProjectTo<WialonUnitDto>(_mapper.ConfigurationProvider)
                                                .FirstAsync(cancellationToken);
        return await Result<WialonUnitDto>.SuccessAsync(data);
    }
}
