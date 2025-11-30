using CleanArchitecture.Blazor.Application.Features.CusPrices.Caching;
using CleanArchitecture.Blazor.Application.Features.CusPrices.DTOs;
using CleanArchitecture.Blazor.Application.Features.CusPrices.Mappers;
using CleanArchitecture.Blazor.Application.Features.CusPrices.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.CusPrices.Queries.GetById;

public class GetCusPriceByIdQuery : ICacheableRequest<Result<CusPriceDto>>
{
    public required int Id { get; set; }
    public string CacheKey => CusPriceCacheKey.GetByIdCacheKey($"{Id}");
    public IEnumerable<string> Tags => CusPriceCacheKey.Tags;

}

public class GetCusPriceByIdQueryHandler :
     IRequestHandler<GetCusPriceByIdQuery, Result<CusPriceDto>>
{
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public GetCusPriceByIdQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public GetCusPriceByIdQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }

    public async Task<Result<CusPriceDto>> Handle(GetCusPriceByIdQuery request, CancellationToken cancellationToken)
    {
        //await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await db.CusPrices.ApplySpecification(new CusPriceByIdSpecification(request.Id))
        //                                        .ProjectTo<CusPriceDto>(_mapper.ConfigurationProvider)
        //                                        .FirstAsync(cancellationToken) ?? throw new NotFoundException($"CusPrice with id: [{request.Id}] not found.");
        //return await Result<CusPriceDto>.SuccessAsync(data);


        var data = await _context.CusPrices.ApplySpecification(new CusPriceByIdSpecification(request.Id))
                                            .ProjectTo()
                                            .FirstAsync(cancellationToken);
        return await Result<CusPriceDto>.SuccessAsync(data);
    }
}
