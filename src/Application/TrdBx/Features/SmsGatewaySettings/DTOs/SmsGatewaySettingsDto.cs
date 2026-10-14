using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Blazor.Application.Features.SmsGatewaySettings.DTOs;

/// <summary>
/// Bound by the settings UI and returned by <c>ISmsGatewaySettingsProvider</c>.
/// The <see cref="RangeAttribute"/>s protect the background service from
/// pathological values (zero-length intervals, negative windows, etc.).
/// </summary>
public class SmsGatewaySettingsDto
{
    public int Id { get; set; }

    // ── connection ───────────────────────────────────────────────

    [Required]
    [Url]
    [Display(Name = "Gateway base URL")]
    public string BaseUrl { get; set; } = "http://127.0.0.1:8081";

    [Display(Name = "API key")]
    public string? ApiKey { get; set; }

    [Range(5, 600)]
    [Display(Name = "HTTP timeout (seconds)")]
    public int TimeoutSeconds { get; set; } = 30;

    // ── inbound master switch + poll cadence ─────────────────────

    [Display(Name = "Enable inbound processing (polling + multipart sweeper)")]
    public bool EnableInboundPolling { get; set; } = true;

    [Range(5, 3600)]
    [Display(Name = "Polling interval (seconds)")]
    public int PollingIntervalSeconds { get; set; } = 30;

    // ── webhook push ─────────────────────────────────────────────

    [Display(Name = "Webhook public URL")]
    public string? WebhookPublicUrl { get; set; }

    [Display(Name = "Webhook secret")]
    public string? WebhookSecret { get; set; }

    // ── multipart sweeper tuning ─────────────────────────────────

    [Range(10, 3600)]
    [Display(Name = "Sweeper interval (seconds)")]
    public int SweeperIntervalSeconds { get; set; } = 60;

    [Range(1, 1440)]
    [Display(Name = "Assembly window (minutes)")]
    public int AssemblyWindowMinutes { get; set; } = 10;

    [Range(1, 1440)]
    [Display(Name = "Orphan threshold (minutes)")]
    public int OrphanThresholdMinutes { get; set; } = 30;

    [Range(1, 365)]
    [Display(Name = "Retention (days)")]
    public int RetentionDays { get; set; } = 30;

    [Range(1, 1000)]
    [Display(Name = "Max groups per sweep")]
    public int MaxGroupsPerSweep { get; set; } = 50;

    // ── audit ────────────────────────────────────────────────────

    public DateTime? UpdatedAt { get; set; }
    public string?   UpdatedBy { get; set; }
}