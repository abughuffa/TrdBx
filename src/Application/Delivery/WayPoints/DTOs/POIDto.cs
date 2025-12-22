
namespace CleanArchitecture.Blazor.Application.Features.WayPoints.DTOs;

[Description("WayPoints")]
public class WayPointDto
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("Latitude")]
    public double Latitude { get; set; } = 0.0;
    [Description("Longitude")]
    public double Longitude { get; set; } = 0.0;


}

