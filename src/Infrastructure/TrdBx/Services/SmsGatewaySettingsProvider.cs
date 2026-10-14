using CleanArchitecture.Blazor.Application.Common.Interfaces;
using CleanArchitecture.Blazor.Application.Features.SmsGatewaySettings.DTOs;
using CleanArchitecture.Blazor.Domain.Entities;
using CleanArchitecture.Blazor.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.Blazor.Infrastructure.TrdBx.Services;

public sealed class SmsGatewaySettingsProvider : ISmsGatewaySettingsProvider
{
    private readonly IApplicationDbContextFactory           _dbFactory;
    private readonly IOptionsMonitor<SmsGatewayOptions>     _bootstrap;
    private readonly ISmsGatewaySettingsCache               _cache;
    private readonly ILogger<SmsGatewaySettingsProvider>    _log;

    public SmsGatewaySettingsProvider(
        IApplicationDbContextFactory dbFactory,
        IOptionsMonitor<SmsGatewayOptions> bootstrap,
        ISmsGatewaySettingsCache cache,
        ILogger<SmsGatewaySettingsProvider> log)
    {
        _dbFactory = dbFactory;
        _bootstrap = bootstrap;
        _cache     = cache;
        _log       = log;
    }

    // ─────────────────────────────────────────────────────────────
    // Read
    // ─────────────────────────────────────────────────────────────

    public async Task<SmsGatewaySettingsDto> GetAsync(CancellationToken ct = default)
    {
        // Fast path: cache hit.
        var cached = _cache.Current;
        if (cached is not null) return cached;

        await using var db = await _dbFactory.CreateAsync(ct);
        var row = await db.SmsGatewaySettings.AsNoTracking()
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(ct);

        if (row is null)
        {
            // First run — seed from appsettings bootstrap.
            var seed = _bootstrap.CurrentValue;

            row = new SmsGatewaySettings
            {
                BaseUrl                = seed.BaseUrl,
                ApiKey                 = seed.ApiKey,
                TimeoutSeconds         = seed.TimeoutSeconds,

                EnableInboundPolling   = seed.EnableInboundPolling,
                PollingIntervalSeconds = seed.PollingIntervalSeconds,

                WebhookPublicUrl       = seed.WebhookPublicUrl,
                WebhookSecret          = seed.WebhookSecret,

                SweeperIntervalSeconds = seed.SweeperIntervalSeconds,
                AssemblyWindowMinutes  = seed.AssemblyWindowMinutes,
                OrphanThresholdMinutes = seed.OrphanThresholdMinutes,
                RetentionDays          = seed.RetentionDays,
                MaxGroupsPerSweep      = seed.MaxGroupsPerSweep,
            };

            db.SmsGatewaySettings.Add(row);
            await db.SaveChangesAsync(ct);

            _log.LogInformation(
                "Seeded SMS gateway settings from appsettings bootstrap: BaseUrl={Base}",
                row.BaseUrl);
        }

        var dto = ToDto(row);
        _cache.Set(dto);
        return dto;
    }

    // ─────────────────────────────────────────────────────────────
    // Write
    // ─────────────────────────────────────────────────────────────

    public async Task UpdateAsync(
        SmsGatewaySettingsDto settings, CancellationToken ct = default)
    {
        await using var db = await _dbFactory.CreateAsync(ct);
        var row = await db.SmsGatewaySettings.FirstOrDefaultAsync(ct);
        if (row is null)
        {
            row = new SmsGatewaySettings();
            db.SmsGatewaySettings.Add(row);
        }

        row.BaseUrl                = settings.BaseUrl;
        row.ApiKey                 = settings.ApiKey;
        row.TimeoutSeconds         = settings.TimeoutSeconds;

        row.EnableInboundPolling   = settings.EnableInboundPolling;
        row.PollingIntervalSeconds = settings.PollingIntervalSeconds;

        row.WebhookPublicUrl       = settings.WebhookPublicUrl;
        row.WebhookSecret          = settings.WebhookSecret;

        row.SweeperIntervalSeconds = settings.SweeperIntervalSeconds;
        row.AssemblyWindowMinutes  = settings.AssemblyWindowMinutes;
        row.OrphanThresholdMinutes = settings.OrphanThresholdMinutes;
        row.RetentionDays          = settings.RetentionDays;
        row.MaxGroupsPerSweep      = settings.MaxGroupsPerSweep;

        await db.SaveChangesAsync(ct);

        var dto = ToDto(row);
        _cache.Set(dto);   // also fires Changed → wakes SmsInboundService

        _log.LogInformation(
            "SMS gateway settings updated: BaseUrl={Base} Polling={Enabled}/{Poll}s Sweeper={SweepInt}s",
            row.BaseUrl,
            row.EnableInboundPolling,
            row.PollingIntervalSeconds,
            row.SweeperIntervalSeconds);
    }

    // ─────────────────────────────────────────────────────────────
    // Mapping
    // ─────────────────────────────────────────────────────────────

    private static SmsGatewaySettingsDto ToDto(SmsGatewaySettings e) => new()
    {
        Id                     = e.Id,

        BaseUrl                = e.BaseUrl,
        ApiKey                 = e.ApiKey,
        TimeoutSeconds         = e.TimeoutSeconds,

        EnableInboundPolling   = e.EnableInboundPolling,
        PollingIntervalSeconds = e.PollingIntervalSeconds,

        WebhookPublicUrl       = e.WebhookPublicUrl,
        WebhookSecret          = e.WebhookSecret,

        SweeperIntervalSeconds = e.SweeperIntervalSeconds,
        AssemblyWindowMinutes  = e.AssemblyWindowMinutes,
        OrphanThresholdMinutes = e.OrphanThresholdMinutes,
        RetentionDays          = e.RetentionDays,
        MaxGroupsPerSweep      = e.MaxGroupsPerSweep,

        UpdatedAt              = e.LastModifiedAt,
        UpdatedBy              = e.LastModifiedById,
    };
}