using CleanArchitecture.Blazor.Application.Features.ServicePrices.Caching;
using CleanArchitecture.Blazor.Application.Features.ServicePrices.DTOs;
using CleanArchitecture.Blazor.Application.Features.ServicePrices.Specifications;
using CleanArchitecture.Blazor.Domain.Entities;

namespace CleanArchitecture.Blazor.Application.Features.ServicePrices.Queries.Pagination;

public class ServicePricesWithPaginationQuery : ServicePriceAdvancedFilter, ICacheableRequest<PaginatedData<ServicePriceDto>>
{
    public override string ToString()
    {
        return $"{OrderBy}, {SortDirection}, {PageNumber}, {PageSize}";
    }
    public string CacheKey => ServicePriceCacheKey.GetPaginationCacheKey($"{this}");
    public IEnumerable<string> Tags => ServicePriceCacheKey.Tags;
    public ServicePriceAdvancedSpecification Specification => new ServicePriceAdvancedSpecification();
}
    
public class ServicePricesWithPaginationQueryHandler :
         IRequestHandler<ServicePricesWithPaginationQuery, PaginatedData<ServicePriceDto>>
{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IMapper _mapper;
    public ServicePricesWithPaginationQueryHandler(
        IApplicationDbContextFactory dbContextFactory,
        IMapper mapper
    )
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
    }

    public async Task<PaginatedData<ServicePriceDto>> Handle(ServicePricesWithPaginationQuery request, CancellationToken cancellationToken)
        {
        await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        var data = await db.ServicePrices.OrderBy($"{request.OrderBy} {request.SortDirection}")
            .ProjectToPaginatedDataAsync<ServicePrice, ServicePriceDto>(request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);
        return data;
    }
}