using CleanArchitecture.Blazor.Application.Features.ServicePrices.Mappers;
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
    //private readonly IApplicationDbContextFactory _dbContextFactory;
    //private readonly IMapper _mapper;
    //public GetAllServicePricesQueryHandler(
    //    IApplicationDbContextFactory dbContextFactory,
    //    IMapper mapper
    //)
    //{
    //    _dbContextFactory = dbContextFactory;
    //    _mapper = mapper;
    //}

    private readonly IApplicationDbContext _context;
    public GetAllServicePricesQueryHandler(
        IApplicationDbContext context
    )
    {
        _context = context;
    }
    public async Task<IEnumerable<ServicePriceDto>> Handle(GetAllServicePricesQuery request, CancellationToken cancellationToken)
    {
        //await using var db = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await db.ServicePrices
        //    .ProjectTo<ServicePriceDto>(_mapper.ConfigurationProvider)
        //    .ToListAsync(cancellationToken);
        //return data;

        var data = await _context.ServicePrices.ProjectTo()
                                           .AsNoTracking()
                                           .ToListAsync(cancellationToken);
        return data;

    }
}


