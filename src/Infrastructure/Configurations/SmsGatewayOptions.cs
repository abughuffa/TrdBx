namespace CleanArchitecture.Blazor.Infrastructure.Configurations;

/// <summary>
/// Bootstrap values read from appsettings.json and used to seed the
/// <see cref="Domain.Entities.SmsGatewaySettings"/> row on first run.
/// After seeding, the DB row is authoritative — appsettings is only
/// consulted again if the row is deleted.
/// </summary>
public class SmsGatewayOptions
{
    public const string SectionName = "SmsGateway";

    // ── connection ───────────────────────────────────────────────
    public string  BaseUrl                { get; set; } =  "http://127.0.0.1:8081";
    public string? ApiKey                 { get; set; }
    public int     TimeoutSeconds         { get; set; } = 30;

    // ── inbound master switch + poll cadence ─────────────────────
    public bool    EnableInboundPolling   { get; set; } = true;
    public int     PollingIntervalSeconds { get; set; } = 30;

    // ── webhook push ─────────────────────────────────────────────
    public string? WebhookPublicUrl       { get; set; }
    public string? WebhookSecret          { get; set; }

    // ── multipart sweeper tuning ─────────────────────────────────
    public int     SweeperIntervalSeconds { get; set; } = 60;
    public int     AssemblyWindowMinutes  { get; set; } = 10;
    public int     OrphanThresholdMinutes { get; set; } = 30;
    public int     RetentionDays          { get; set; } = 30;
    public int     MaxGroupsPerSweep      { get; set; } = 50;
}
