
using CleanArchitecture.Blazor.Application.Features.Shipments.DTOs;
using CleanArchitecture.Blazor.Application.Features.Shipments.Caching;
using CleanArchitecture.Blazor.Application.Features.Shipments.Mappers;
using CleanArchitecture.Blazor.Application.Features.Shipments.Specifications;

namespace CleanArchitecture.Blazor.Application.Features.Shipments.Queries.GetById;

public class GetShipmentByIdQuery : ICacheableRequest<Result<ShipmentDto>>
{
   public required int Id { get; set; }
   public string CacheKey => ShipmentCacheKey.GetByIdCacheKey($"{Id}");
   public IEnumerable<string>? Tags => ShipmentCacheKey.Tags;
}

public class GetShipmentByIdQueryHandler :
     IRequestHandler<GetShipmentByIdQuery, Result<ShipmentDto>>
{
    private readonly IApplicationDbContext _context;

    public GetShipmentByIdQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<ShipmentDto>> Handle(GetShipmentByIdQuery request, CancellationToken cancellationToken)
    {
        //var data = await _context.Shipments.ApplySpecification(new ShipmentByIdSpecification(request.Id))
        //                                        .ProjectTo()
        //                                        .FirstAsync(cancellationToken);
        //return await Result<ShipmentDto>.SuccessAsync(data);

        var shipment = await _context.Shipments
           .Include(s => s.VehicleTypes)
               .ThenInclude(svt => svt.VehicleType)
           .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (shipment == null)
            return await Result<ShipmentDto>.FailureAsync($"Shipment with ID {request.Id} not found.");

        var dto = shipment.ToDtoWithVehicleTypes();
        return await Result<ShipmentDto>.SuccessAsync(dto);
    }
}
