
using CleanArchitecture.Blazor.Application.Features.VehicleTypes.DTOs;
using CleanArchitecture.Blazor.Application.Features.VehicleTypes.Mappers;
using CleanArchitecture.Blazor.Application.Features.VehicleTypes.Caching;

namespace CleanArchitecture.Blazor.Application.Features.VehicleTypes.Queries.GetAll;

public class GetAllVehicleTypesQuery : ICacheableRequest<IEnumerable<VehicleTypeDto>>
{
   public string CacheKey => VehicleTypeCacheKey.GetAllCacheKey;
   public IEnumerable<string>? Tags => VehicleTypeCacheKey.Tags;
}

public class GetAllVehicleTypesQueryHandler :
     IRequestHandler<GetAllVehicleTypesQuery, IEnumerable<VehicleTypeDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllVehicleTypesQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<VehicleTypeDto>> Handle(GetAllVehicleTypesQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.VehicleTypes.ProjectTo()
                                                .AsNoTracking()
                                                .ToListAsync(cancellationToken);
        return data;
    }
}


