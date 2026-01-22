using CleanArchitecture.Blazor.Domain.Common.Entities;
using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Identity;

namespace CleanArchitecture.Blazor.Domain.Entities;

public class POI : BaseAuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public double Latitude { get; set; } = 32.8877;
    public double Longitude { get; set; } = 13.1872;
}
public class WayPoint : BaseEntity
{
    public int ShipmentId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class BidRecord : BaseAuditableEntity
{
    public int ShipmentId { get; set; }
    public string TransporterId { get; set; } = string.Empty;
    public decimal Price { get; set; } = 0.0m;
    public Shipment? Shipment { get; set; }
    public ApplicationUser? Transporter { get; set; }
}
public class Shipment : BaseAuditableEntity
{
    public string ShipmentNo { get; set; } = string.Empty;
    public ShipmentStatus ShipmentStatus { get; set; } = ShipmentStatus.JustCreated;
    public virtual ICollection<WayPoint> WayPoints { get; set; } = new HashSet<WayPoint>();
    public decimal Price { get; set; } = 0.0m;
    public bool IsBidable { get; set; } = false;
    public int? VehicleId { get; set; }
    public Vehicle? Vehicle { get; set; }
    // Navigation property
    public virtual ICollection<ShipmentVehicleType> VehicleTypes { get; set; } = new HashSet<ShipmentVehicleType>();
    public virtual ICollection<BidRecord> BidRecords { get; set; } = new HashSet<BidRecord>();
}
// Junction table
public class ShipmentVehicleType
{
    public int ShipmentId { get; set; }
    public int VehicleTypeId { get; set; }
    public virtual Shipment Shipment { get; set; } = null!;
    public virtual VehicleType VehicleType { get; set; } = null!;
}

public class VehicleType : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public byte Image { get; set; }
    public virtual ICollection<ShipmentVehicleType> Shipments { get; set; } = new HashSet<ShipmentVehicleType>();

}
public class Vehicle : BaseAuditableEntity
{
    public string VehicleNo { get; set; } = string.Empty;
    public int VehicleTypeId { get; set; }
    public string? DriverId { get; set; }
    public VehicleType VehicleType { get; set; }
    public ApplicationUser? Driver { get; set; }

}








