// using CleanArchitecture.Blazor.Application.Features.ServicePrices.Mappers;
using CleanArchitecture.Blazor.Application.Features.ServicePrices.Caching;
using CleanArchitecture.Blazor.Application.Features.ServicePrices.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.ServicePrices.Queries.GetAll;

public class GetAllServicePricesQuery : ICacheableRequest<IEnumerable<ServicePriceDto>>
{
   public string CacheKey => ServicePriceCacheKey.GetAllCacheKey;
    public IEnumerable<string> Tags => ServicePriceCacheKey.Tags;
}

public class GetAllServicePricesQueryHandler :
     IRequestHandler<GetAllServicePricesQuery, IEnumerable<ServicePriceDto>>
{
        private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly TypeAdapterConfig _typeAdapterConfig;
    public GetAllServicePricesQueryHandler(
        TypeAdapterConfig typeAdapterConfig,
        IApplicationDbContextFactory dbContextFactory)
    {
        _typeAdapterConfig = typeAdapterConfig;
        _dbContextFactory = dbContextFactory;
    }
    public async ValueTask<IEnumerable<ServicePriceDto>> Handle(GetAllServicePricesQuery request, CancellationToken cancellationToken)
    {
        await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await db.ServicePrices
        //    .ProjectTo<ServicePriceDto>(_mapper.ConfigurationProvider)
        //    .ToListAsync(cancellationToken);
        //return data;

        var data = await _context.ServicePrices.ProjectToType<ServicePriceDto>(_typeAdapterConfig)
                                           .AsNoTracking()
                                           .ToListAsync(cancellationToken);
        return data;

    }
}


