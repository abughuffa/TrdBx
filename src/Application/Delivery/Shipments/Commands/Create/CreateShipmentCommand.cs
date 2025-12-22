

using CleanArchitecture.Blazor.Application.Features.Shipments.Caching;
using CleanArchitecture.Blazor.Application.Features.Shipments.Mappers;
using CleanArchitecture.Blazor.Application.Features.WayPoints.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.Shipments.Commands.Create;

public class CreateShipmentCommand: ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("ShipmentNo")]
    public string ShipmentNo { get; set; }
    [Description("WayPoints")]
    public List<WayPointDto> WayPoints { get; set; }
    //[Description("StartLocation")]
    //public string StartLocation { get; set; } = $"0.0,0.0";
    //[Description("EndLocation")]
    //public string EndLocation { get; set; } = $"0.0,0.0";
    [Description("Price")]
    public decimal Price { get; set; } = 0.0m;
    [Description("IsBidable")]
    public bool IsBidable { get; set; } = false;
    [Description("RecVehicleType")]
    public int[] RecVehicleType { get; set; } = Array.Empty<int>();

    public string CacheKey => ShipmentCacheKey.GetAllCacheKey;
      public IEnumerable<string>? Tags => ShipmentCacheKey.Tags;
}
    
    public class CreateShipmentCommandHandler : IRequestHandler<CreateShipmentCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        public CreateShipmentCommandHandler(
            IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result<int>> Handle(CreateShipmentCommand request, CancellationToken cancellationToken)
        {
        var shipment = request.ToEntity();

        // Add vehicle types
        foreach (var vehicleTypeId in request.RecVehicleType ?? Array.Empty<int>())
        {
            var vehicleTypeExists = await _context.VehicleTypes
                .AnyAsync(vt => vt.Id == vehicleTypeId, cancellationToken);

            if (vehicleTypeExists)
            {
                shipment.VehicleTypes.Add(new ShipmentVehicleType
                {
                    VehicleTypeId = vehicleTypeId
                });
            }
        }

        shipment.AddDomainEvent(new ShipmentCreatedEvent(shipment));

        _context.Shipments.Add(shipment);

        await _context.SaveChangesAsync(cancellationToken);

        return await Result<int>.SuccessAsync(shipment.Id);
    }
}

