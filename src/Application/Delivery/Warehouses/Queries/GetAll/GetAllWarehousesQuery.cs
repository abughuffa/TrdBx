
using CleanArchitecture.Blazor.Application.Features.Warehouses.DTOs;
using CleanArchitecture.Blazor.Application.Features.Warehouses.Mappers;
using CleanArchitecture.Blazor.Application.Features.Warehouses.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Warehouses.Queries.GetAll;

public class GetAllWarehousesQuery : ICacheableRequest<IEnumerable<WarehouseDto>>
{
   public string CacheKey => WarehouseCacheKey.GetAllCacheKey;
   public IEnumerable<string>? Tags => WarehouseCacheKey.Tags;
}

public class GetAllWarehousesQueryHandler :
     IRequestHandler<GetAllWarehousesQuery, IEnumerable<WarehouseDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllWarehousesQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WarehouseDto>> Handle(GetAllWarehousesQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.Warehouses.ProjectTo()
                                                .AsNoTracking()
                                                .ToListAsync(cancellationToken);
        return data;
    }
}


