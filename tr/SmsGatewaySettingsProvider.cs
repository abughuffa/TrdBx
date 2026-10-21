using CleanArchitecture.Blazor.Application.Common.Interfaces;
using CleanArchitecture.Blazor.Application.Features.SmsGatewaySettings.DTOs;

using CleanArchitecture.Blazor.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.Blazor.Infrastructure.TrdBx.Services;

public sealed class SmsGatewaySettingsProvider : ISmsGatewaySettingsProvider
{
    private readonly IApplicationDbContextFactory _dbFactory;
    private readonly IOptionsMonitor<SmsGatewayOptions> _bootstrap;
    private readonly ISmsGatewaySettingsCache _cache;
    private readonly ILogger<SmsGatewaySettingsProvider> _log;

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
            // Seed from appsettings bootstrap.
            var seed = _bootstrap.CurrentValue;
            row = new SmsGatewaySettings
            {
                BaseUrl                = seed.BaseUrl,
                ApiKey                 = seed.ApiKey,
                EnableInboundPolling   = seed.EnableInboundPolling,
                PollingIntervalSeconds = seed.PollingIntervalSeconds,
                WebhookPublicUrl       = seed.WebhookPublicUrl,
                WebhookSecret          = seed.WebhookSecret,
                TimeoutSeconds         = seed.TimeoutSeconds,
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

    public async Task UpdateAsync(SmsGatewaySettingsDto settings, CancellationToken ct = default)
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
        row.EnableInboundPolling   = settings.EnableInboundPolling;
        row.PollingIntervalSeconds = settings.PollingIntervalSeconds;
        row.WebhookPublicUrl       = settings.WebhookPublicUrl;
        row.WebhookSecret          = settings.WebhookSecret;
        row.TimeoutSeconds         = settings.TimeoutSeconds;

        await db.SaveChangesAsync(ct);

        var dto = ToDto(row);
        _cache.Set(dto);   // also fires Changed

        _log.LogInformation(
            "SMS gateway settings updated: BaseUrl={Base} Polling={Enabled}/{Interval}s",
            row.BaseUrl, row.EnableInboundPolling, row.PollingIntervalSeconds);
    }

    private static SmsGatewaySettingsDto ToDto(SmsGatewaySettings e) => new()
    {
        Id                     = e.Id,
        BaseUrl                = e.BaseUrl,
        ApiKey                 = e.ApiKey,
        EnableInboundPolling   = e.EnableInboundPolling,
        PollingIntervalSeconds = e.PollingIntervalSeconds,
        WebhookPublicUrl       = e.WebhookPublicUrl,
        WebhookSecret          = e.WebhookSecret,
        TimeoutSeconds         = e.TimeoutSeconds,
        UpdatedAt              = e.LastModifiedAt,
        UpdatedBy              = e.LastModifiedById,
    };
}