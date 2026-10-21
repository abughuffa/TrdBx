// using CleanArchitecture.Blazor.Application.Features.SPackages.Mappers;
using CleanArchitecture.Blazor.Application.Features.SPackages.Caching;
using CleanArchitecture.Blazor.Application.Features.SPackages.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.SPackages.Queries.GetAll;

public class GetAllSPackagesQuery : ICacheableRequest<IEnumerable<SPackageDto>>
{
   public string CacheKey => SPackageCacheKey.GetAllCacheKey;
    public IEnumerable<string> Tags => SPackageCacheKey.Tags;
}

public class GetAllSPackagesQueryHandler :
     IRequestHandler<GetAllSPackagesQuery, IEnumerable<SPackageDto>>
{
       private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly TypeAdapterConfig _typeAdapterConfig;
    public GetAllSPackagesQueryHandler(
        TypeAdapterConfig typeAdapterConfig,
        IApplicationDbContextFactory dbContextFactory)
    {
        _typeAdapterConfig = typeAdapterConfig;
        _dbContextFactory = dbContextFactory;
    }
    public async ValueTask<IEnumerable<SPackageDto>> Handle(GetAllSPackagesQuery request, CancellationToken cancellationToken)
    {
        await using var _context = await _dbContextFactory.CreateAsync(cancellationToken);
        //var data = await db.SPackages
        //    .ProjectTo<SPackageDto>(_mapper.ConfigurationProvider)
        //    .ToListAsync(cancellationToken);
        //return data;

        var data = await _context.SPackages.Include(s=>s.SProvider).ProjectToType<SPackageDto>(_typeAdapterConfig)
                                           .AsNoTracking()
                                           .ToListAsync(cancellationToken);
        return data;
    }
}


