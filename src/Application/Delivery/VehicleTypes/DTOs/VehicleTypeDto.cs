
namespace CleanArchitecture.Blazor.Application.Features.VehicleTypes.DTOs;

[Description("VehicleTypes")]
public class VehicleTypeDto
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("Name")]
    public string Name { get; set; } = string.Empty;
    [Description("Image")]
    public byte Image { get; set; }



}

