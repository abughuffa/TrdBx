// using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.WialonUnits.Mappers;
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.WialonUnits.Caching;
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.WialonUnits.DTOs;
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.WialonUnits.Specifications;


namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.WialonUnits.Queries.GetById;

public class GetWialonUnitByIdQuery : ICacheableRequest<Result<WialonUnitDto>>
{
    public required int Id { get; set; }
    public string CacheKey => WialonUnitCacheKey.GetByIdCacheKey($"{Id}");
     public IEnumerable<string> Tags => WialonUnitCacheKey.Tags;
}

public class GetWialonUnitByIdQueryHandler :
     IRequestHandler<GetWialonUnitByIdQuery, Result<WialonUnitDto>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public GetWialonUnitByIdQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}
        private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly TypeAdapterConfig _typeAdapterConfig;
    public GetWialonUnitByIdQueryHandler(
        TypeAdapterConfig typeAdapterConfig,
        IApplicationDbContextFactory dbContextFactory)
    {
        _typeAdapterConfig = typeAdapterConfig;
        _dbContextFactory = dbContextFactory;
    }

    public async ValueTask<Result<WialonUnitDto>> Handle(GetWialonUnitByIdQuery request, CancellationToken cancellationToken)
    {
        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        var data = await context.WialonUnits.ApplySpecification(new WialonUnitByIdSpecification(request.Id))
                              .ProjectToType<WialonUnitDto>(_typeAdapterConfig)
                                .FirstAsync(cancellationToken) ?? throw new NotFoundException($"WialonUnit with id: [{request.Id}] not found.");
        return await Result<WialonUnitDto>.SuccessAsync(data);
    }
}
