

using CleanArchitecture.Blazor.Application.Features.Shipments.Caching;
using CleanArchitecture.Blazor.Application.Features.Shipments.Mappers;

namespace CleanArchitecture.Blazor.Application.Features.Shipments.Commands.Update;

public class UpdateShipmentCommand: ICacheInvalidatorRequest<Result<int>>
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("ShipmentNo")]
    public string ShipmentNo { get; set; }
    [Description("StartLocation")]
    public string StartLocation { get; set; }
    [Description("EndLocation")]
    public string EndLocation { get; set; }
    [Description("Price")]
    public decimal Price { get; set; }
    [Description("IsBidable")]
    public bool IsBidable { get; set; }
    [Description("RecVehicleType")]
    public int[] RecVehicleType { get; set; }

    public string CacheKey => ShipmentCacheKey.GetAllCacheKey;
      public IEnumerable<string>? Tags => ShipmentCacheKey.Tags;

}

public class UpdateShipmentCommandHandler : IRequestHandler<UpdateShipmentCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    public UpdateShipmentCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Result<int>> Handle(UpdateShipmentCommand request, CancellationToken cancellationToken)
    {

        var shipment = await _context.Shipments
              .Include(s => s.VehicleTypes)
              .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (shipment == null)
            return await Result<int>.FailureAsync($"Shipment with ID {request.Id} not found.");

        // Update basic properties using Mapperly
        request.ToEntity(shipment);

        // Update vehicle types
        await UpdateVehicleTypes(shipment, request.RecVehicleType, cancellationToken);

        shipment.AddDomainEvent(new ShipmentUpdatedEvent(shipment));

        await _context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(shipment.Id);
    }

    private async Task UpdateVehicleTypes(Shipment shipment, int[] newVehicleTypeIds, CancellationToken cancellationToken)
    {
        // Remove vehicle types that are no longer selected
        var vehicleTypesToRemove = shipment.VehicleTypes
            .Where(vt => !newVehicleTypeIds.Contains(vt.VehicleTypeId))
            .ToList();

        foreach (var vehicleType in vehicleTypesToRemove)
        {
            shipment.VehicleTypes.Remove(vehicleType);
        }

        // Add new vehicle types
        var existingVehicleTypeIds = shipment.VehicleTypes.Select(vt => vt.VehicleTypeId).ToHashSet();
        var vehicleTypesToAdd = newVehicleTypeIds
            .Where(id => !existingVehicleTypeIds.Contains(id))
            .ToList();

        foreach (var vehicleTypeId in vehicleTypesToAdd)
        {
            var vehicleTypeExists = await _context.VehicleTypes
                .AnyAsync(vt => vt.Id == vehicleTypeId, cancellationToken);

            if (vehicleTypeExists)
            {
                shipment.VehicleTypes.Add(new ShipmentVehicleType
                {
                    ShipmentId = shipment.Id,
                    VehicleTypeId = vehicleTypeId
                });
            }
        }
    }
}
