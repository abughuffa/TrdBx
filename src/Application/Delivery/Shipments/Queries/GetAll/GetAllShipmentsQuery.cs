
using CleanArchitecture.Blazor.Application.Features.Shipments.DTOs;
using CleanArchitecture.Blazor.Application.Features.Shipments.Mappers;
using CleanArchitecture.Blazor.Application.Features.Shipments.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Shipments.Queries.GetAll;

public class GetAllShipmentsQuery : ICacheableRequest<IEnumerable<ShipmentDto>>
{
   public string CacheKey => ShipmentCacheKey.GetAllCacheKey;
   public IEnumerable<string>? Tags => ShipmentCacheKey.Tags;
}

public class GetAllShipmentsQueryHandler :
     IRequestHandler<GetAllShipmentsQuery, IEnumerable<ShipmentDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllShipmentsQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ShipmentDto>> Handle(GetAllShipmentsQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.Shipments.ProjectToDto()
                                                .AsNoTracking()
                                                .ToListAsync(cancellationToken);
        return data;
    }
}


