
namespace CleanArchitecture.Blazor.Application.Features.Drivers.DTOs;

[Description("Drivers")]
public class DriverDto
{
    [Description("UserId")]
    public string UserId { get; set; }
    [Description("DisplayName")]
    public string DisplayName { get;set;} 
    [Description("Email")]
    public string? Email {get;set;} 
    [Description("Phone number")]
    public string? PhoneNumber {get;set;} 
}

