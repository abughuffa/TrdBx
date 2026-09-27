namespace CleanArchitecture.Blazor.Infrastructure.Configurations;

/// <summary>
/// Configuration for talking to the Android SMS Gateway.
/// Bound from the "SmsGateway" section in appsettings.json.
/// </summary>
public class SmsGatewayOptions
{
    public const string SectionName = "SmsGateway";

    /// <summary>
    /// Base URL of the gateway HTTP server.
    ///   Wi-Fi mode     → "http://<phone-lan-ip>:8080"
    ///   ADB forward    → "http://127.0.0.1:8081"  (after `adb forward tcp:8081 tcp:8080`)
    /// </summary>
    public string BaseUrl { get; set; } = "http://192.168.1.42:8080";

    /// <summary>
    /// Optional shared secret. When set, the client sends it as the
    /// X-Api-Key header on every request. Must match the value configured
    /// on the phone's Wi-Fi settings screen.
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    /// When true, a hosted background service polls GET /inbox on an
    /// interval and persists new messages. Useful when the Blazor host
    /// is not publicly reachable and the phone cannot push a webhook.
    /// </summary>
    public bool EnableInboundPolling { get; set; } = true;

    /// <summary>
    /// Seconds between /inbox polls. Ignored if EnableInboundPolling is false.
    /// </summary>
    public int PollingIntervalSeconds { get; set; } = 30;

    /// <summary>
    /// Public URL the phone can reach to deliver inbound SMS push notifications.
    /// Leave null on a LAN/dev box and rely on polling instead.
    /// Example: "https://yourhost/api/sms/webhook"
    /// </summary>
    public string? WebhookPublicUrl { get; set; }

    /// <summary>
    /// HMAC-SHA256 secret used to sign webhook payloads. The receiver must
    /// verify the X-Signature header against this value.
    /// </summary>
    public string? WebhookSecret { get; set; }

    /// <summary>
    /// Per-request HTTP timeout in seconds.
    /// </summary>
    public int TimeoutSeconds { get; set; } = 15;
}