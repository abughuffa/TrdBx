
namespace CleanArchitecture.Blazor.Application.Features.Vehicles.DTOs;

[Description("Vehicles")]
public class VehicleDto
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("VehicleNo")]
    public string VehicleNo { get; set; } = string.Empty;
    [Description("VehicleTypeId")]
    public int VehicleTypeId { get; set; }
    [Description("DriverId")]
    public string DriverId { get; set; }



}

