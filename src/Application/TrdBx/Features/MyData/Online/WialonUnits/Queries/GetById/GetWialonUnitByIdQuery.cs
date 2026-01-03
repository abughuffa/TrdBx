using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.WialonUnits.Mappers;
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
    private readonly IApplicationDbContext _context;
    public GetWialonUnitByIdQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<Result<WialonUnitDto>> Handle(GetWialonUnitByIdQuery request, CancellationToken cancellationToken)
    {
        //await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await _context.WialonUnits.ApplySpecification(new WialonUnitByIdSpecification(request.Id))
        //                                      .ProjectTo<WialonUnitDto>(_mapper.ConfigurationProvider)
        //                                        .FirstAsync(cancellationToken);
        //return await Result<WialonUnitDto>.SuccessAsync(data);

        var data = await _context.WialonUnits.ApplySpecification(new WialonUnitByIdSpecification(request.Id))
                                .ProjectTo()
                                .FirstAsync(cancellationToken) ?? throw new NotFoundException($"WialonUnit with id: [{request.Id}] not found.");
        return await Result<WialonUnitDto>.SuccessAsync(data);
    }
}
