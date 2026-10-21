using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Blazor.Domain.Common.Entities;
using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Identity;

namespace CleanArchitecture.Blazor.Domain.Entities;


public class SmsCursor : BaseEntity
{
    public string Key   { get; set; } = null!;
    public long Value { get; set; } = 0L!;
}

/// <summary>
/// Runtime-editable SMS gateway configuration. Single row (Id = 1).
/// Seeded once from appsettings.json (SmsGatewayOptions) on first read;
/// afterwards the DB row is the source of truth.
/// </summary>
public class SmsGatewaySettings : BaseAuditableEntity
{   
    // ── connection ───────────────────────────────────────────────
    public string  BaseUrl { get; set; } = "http://127.0.0.1:8081";
    public string? ApiKey                 { get; set; }
    public int     TimeoutSeconds         { get; set; } = 30;

    // ── inbound master switch + poll cadence ─────────────────────
    public bool    EnableInboundPolling   { get; set; } = true;
    public int     PollingIntervalSeconds { get; set; } = 30;

    // ── webhook push (independent of the polling switch) ─────────
    public string? WebhookPublicUrl       { get; set; }
    public string? WebhookSecret          { get; set; }

    // ── multipart sweeper tuning (only runs while polling is on) ─
    public int     SweeperIntervalSeconds { get; set; } = 60;
    public int     AssemblyWindowMinutes  { get; set; } = 10;
    public int     OrphanThresholdMinutes { get; set; } = 30;
    public int     RetentionDays          { get; set; } = 30;
    public int     MaxGroupsPerSweep      { get; set; } = 50;
}

