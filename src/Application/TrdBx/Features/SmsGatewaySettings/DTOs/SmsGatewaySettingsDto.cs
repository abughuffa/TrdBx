namespace CleanArchitecture.Blazor.Application.Features.SmsGatewaySettings.DTOs;

public class SmsGatewaySettingsDto
{
    public int     Id                    { get; set; }
    public string  BaseUrl               { get; set; } = "http://127.0.0.1:8081";
    public string? ApiKey                { get; set; }
    public bool    EnableInboundPolling  { get; set; } = true;
    public int     PollingIntervalSeconds { get; set; } = 30;
    public string? WebhookPublicUrl      { get; set; }
    public string? WebhookSecret         { get; set; }
    public int     TimeoutSeconds        { get; set; } = 5;

    public DateTime? UpdatedAt { get; set; }
    public string?   UpdatedBy { get; set; }
}