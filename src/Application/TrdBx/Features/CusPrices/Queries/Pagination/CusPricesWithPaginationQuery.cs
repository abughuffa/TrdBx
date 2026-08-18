using CleanArchitecture.Blazor.Application.Features.CusPrices.Caching;
using CleanArchitecture.Blazor.Application.Features.CusPrices.DTOs;
using CleanArchitecture.Blazor.Application.Features.CusPrices.Specifications;

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

         private readonly IApplicationDbContextFactory _dbContextFactory;
        private readonly TypeAdapterConfig _typeAdapterConfig;
        public CusPricesWithPaginationQueryHandler(
            TypeAdapterConfig typeAdapterConfig,
            IApplicationDbContextFactory dbContextFactory)
        {
            _typeAdapterConfig = typeAdapterConfig;
            _dbContextFactory = dbContextFactory;
        }


    public async ValueTask<PaginatedData<CusPriceDto>> Handle(CusPricesWithPaginationQuery request, CancellationToken cancellationToken)
    {


        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        var data = await context.CusPrices.Include(s => s.TrackingUnitModel).Include(s => s.Customer).OrderBy($"{request.OrderBy} {request.SortDirection}")
                                                 .ProjectToPaginatedDataAsync<CusPrice, CusPriceDto>(request.Specification,
                                                    request.PageNumber,
                                                    request.PageSize,
                                                    _typeAdapterConfig,
                                                    cancellationToken);
        return data;

    }
}