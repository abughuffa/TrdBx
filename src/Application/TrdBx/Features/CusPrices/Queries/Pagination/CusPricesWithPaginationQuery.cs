using CleanArchitecture.Blazor.Application.Features.CusPrices.Caching;
using CleanArchitecture.Blazor.Application.Features.CusPrices.DTOs;
using CleanArchitecture.Blazor.Application.Features.CusPrices.Mappers;
using CleanArchitecture.Blazor.Application.Features.CusPrices.Specifications;
using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.CusPrices.Queries.Pagination;

public class CusPricesWithPaginationQuery : CusPriceAdvancedFilter, ICacheableRequest<PaginatedData<CusPriceDto>>
{
    public override string ToString()
    {
        return $"Listview:{ListView}: Search:{Keyword}, {OrderBy}, {SortDirection}, {PageNumber}, {PageSize}";
    }

    public string CacheKey => CusPriceCacheKey.GetPaginationCacheKey($"{this}");
    public IEnumerable<string>? Tags => CusPriceCacheKey.Tags;
    public CusPriceAdvancedSpecification Specification => new CusPriceAdvancedSpecification(this);

}

public class CusPricesWithPaginationQueryHandler :
         IRequestHandler<CusPricesWithPaginationQuery, PaginatedData<CusPriceDto>>
{

    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public CusPricesWithPaginationQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;

    public CusPricesWithPaginationQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedData<CusPriceDto>> Handle(CusPricesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        //await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await db.CusPrices.OrderBy($"{request.OrderBy} {request.SortDirection}")
        //    .ProjectToPaginatedDataAsync<CusPrice, CusPriceDto>(request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);
        //return data;

        var data = await _context.CusPrices.OrderBy($"{request.OrderBy} {request.SortDirection}")
                                                  .ProjectToPaginatedDataAsync(request.Specification,
                                                                               request.PageNumber,
                                                                               request.PageSize,
                                                                               Mapper.ToDto,
                                                                               cancellationToken);
        return data;

    }
}