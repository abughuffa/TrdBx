// using CleanArchitecture.Blazor.Application.Features.SProviders.Mappers;
using CleanArchitecture.Blazor.Application.Features.SProviders.Caching;
using CleanArchitecture.Blazor.Application.Features.SProviders.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.SProviders.Queries.GetAll;

public class GetAllSProvidersQuery : ICacheableRequest<IEnumerable<SProviderDto>>
{
   public string CacheKey => SProviderCacheKey.GetAllCacheKey;
    public IEnumerable<string> Tags => SProviderCacheKey.Tags;
}

public class GetAllSProvidersQueryHandler :
     IRequestHandler<GetAllSProvidersQuery, IEnumerable<SProviderDto>>
{
       private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly TypeAdapterConfig _typeAdapterConfig;
    public GetAllSProvidersQueryHandler(
        TypeAdapterConfig typeAdapterConfig,
        IApplicationDbContextFactory dbContextFactory)
    {
        _typeAdapterConfig = typeAdapterConfig;
        _dbContextFactory = dbContextFactory;
    }
    public async ValueTask<IEnumerable<SProviderDto>> Handle(GetAllSProvidersQuery request, CancellationToken cancellationToken)
    {
        await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await db.SProviders
        //    .ProjectTo<SProviderDto>(_mapper.ConfigurationProvider)
        //    .ToListAsync(cancellationToken);
        //return data;

        var data = await _context.SProviders.ProjectToType<SProviderDto>(_typeAdapterConfig)
                                           .AsNoTracking()
                                           .ToListAsync(cancellationToken);
        return data;
    }
}


