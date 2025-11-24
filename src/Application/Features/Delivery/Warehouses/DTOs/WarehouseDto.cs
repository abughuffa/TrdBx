
namespace CleanArchitecture.Blazor.Application.Features.Warehouses.DTOs;

[Description("Warehouses")]
public class WarehouseDto
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("Name")]
    public string Name { get; set; } = string.Empty;
    //[Description("Location")]
    //public string Location { get; set; } = $"0.0,0.0";
    [Description("Latitude")]
    public double Latitude { get; set; } = 0.0;
    [Description("Longitude")]
    public double Longitude { get; set; } = 0.0;


}

