
using CleanArchitecture.Blazor.Application.Features.Vehicles.DTOs;
using CleanArchitecture.Blazor.Application.Features.Vehicles.Mappers;
using CleanArchitecture.Blazor.Application.Features.Vehicles.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Vehicles.Queries.GetAll;

public class GetAllVehiclesQuery : ICacheableRequest<IEnumerable<VehicleDto>>
{
   public string CacheKey => VehicleCacheKey.GetAllCacheKey;
   public IEnumerable<string>? Tags => VehicleCacheKey.Tags;
}

public class GetAllVehiclesQueryHandler :
     IRequestHandler<GetAllVehiclesQuery, IEnumerable<VehicleDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllVehiclesQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<VehicleDto>> Handle(GetAllVehiclesQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.Vehicles.ProjectTo()
                                                .AsNoTracking()
                                                .ToListAsync(cancellationToken);
        return data;
    }
}


