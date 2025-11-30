using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.Shipments.DTOs;

[Description("Shipments")]
public class ShipmentDto
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("ShipmentNo")]
    public string ShipmentNo { get; set; }
    [Description("ShipmentStatus")]
    public ShipmentStatus ShipmentStatus { get; set; } = ShipmentStatus.JustCreated;
    [Description("StartLocation")]
    public string StartLocation { get; set; } = $"0.0,0.0";
    [Description("EndLocation")]
    public string EndLocation { get; set; } = $"0.0,0.0";
    [Description("Price")]
    public decimal Price { get; set; } = 0.0m;
    [Description("IsBidable")]
    public bool IsBidable { get; set; } = false;
    [Description("VehicleId")]
    public int? VehicleId { get; set; }
    //[Description("RecVehicleType")]
    //public int[] RecVehicleType { get; set; } = Array.Empty<int>();

    // For form binding
    public int[] RecVehicleType { get; set; } = Array.Empty<int>();

    // For display
    public string[] RecVehicleTypeNames { get; set; } = Array.Empty<string>();


}

