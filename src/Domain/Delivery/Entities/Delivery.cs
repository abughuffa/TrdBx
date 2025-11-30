using CleanArchitecture.Blazor.Domain.Common.Entities;
using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Identity;

namespace CleanArchitecture.Blazor.Domain.Entities;

//public class Location : BaseAuditableEntity
//{
//    public decimal Lon { get; set; } = 0.0m;
//    public decimal Lat { get; set; } = 0.0m;
//}

public class Warehouse : BaseAuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public double Latitude { get; set; } = 32.8877;
    public double Longitude { get; set; } = 13.1872;
    //public string Coordinate { get; set; } = "0.0,0.0";
    
}




public class Vehicle : BaseAuditableEntity
{
    public string VehicleNo { get; set; } = string.Empty;
    public int VehicleTypeId { get; set; }
    public string? DriverId { get; set; }
    public VehicleType VehicleType { get; set; }
    public ApplicationUser? Driver { get; set; }
    
}






public class VehicleType : BaseAuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public byte Image { get; set; }

    public virtual ICollection<ShipmentVehicleType> Shipments { get; set; } = new HashSet<ShipmentVehicleType>();

}

public class Shipment : BaseAuditableEntity
{
    public string ShipmentNo { get; set; }
    public ShipmentStatus ShipmentStatus { get; set; } = ShipmentStatus.JustCreated;
    public string StartLocation { get; set; } = $"0.0,0.0";
    public string EndLocation { get; set; } = $"0.0,0.0";
    public decimal Price { get; set; } = 0.0m;
    public bool IsBidable { get; set; } = false;
    public int? VehicleId { get; set; }
    //public int[] RecVehicleTypes { get; set; } = Array.Empty<int>();
    public Vehicle? Vehicle { get; set; }

    // Navigation property
    public virtual ICollection<ShipmentVehicleType> VehicleTypes { get; set; } = new HashSet<ShipmentVehicleType>();
}

// Junction table
public class ShipmentVehicleType
{
    public int ShipmentId { get; set; }
    public int VehicleTypeId { get; set; }

    public virtual Shipment Shipment { get; set; } = null!;
    public virtual VehicleType VehicleType { get; set; } = null!;
}

