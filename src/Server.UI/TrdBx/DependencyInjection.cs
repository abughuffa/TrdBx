using CleanArchitecture.Blazor.Application.Features.SmsGatewaySettings.DTOs;
using CleanArchitecture.Blazor.Server.UI.Endpoints;
using CleanArchitecture.Blazor.Server.UI.Services;

namespace CleanArchitecture.Blazor.Server.UI;

/// <summary>
/// SMS Gateway wiring: hosted poller, webhook endpoint, and best-effort
/// webhook registration on startup. Split out from the main
/// DependencyInjection partial so SMS concerns stay isolated.
/// </summary>
public static partial class DependencyInjection
{
        public static IServiceCollection AddSmsGatewayServices(
        this IServiceCollection services)
    {
        services.AddHostedService<SmsInboundService>();
        // services.AddHostedService<SmsInboundPartSweeper>();
        return services;
    }

    public static WebApplication MapSmsGatewayEndpoints(this WebApplication app)
    {
        app.MapSmsWebhook();
        return app;
    }

public static WebApplication RegisterSmsWebhookOnStartup(this WebApplication app)
{
    app.Lifetime.ApplicationStarted.Register(() =>
    {
        _ = Task.Run(async () =>
        {
            using var scope = app.Services.CreateScope();
            var sp    = scope.ServiceProvider;
            var cache = sp.GetRequiredService<ISmsGatewaySettingsCache>();
            var gw    = sp.GetRequiredService<ISmsGatewayClient>();
            var prov  = sp.GetRequiredService<ISmsGatewaySettingsProvider>();
            var log   = sp.GetRequiredService<ILogger<Program>>();

            // Initial read to populate the cache if necessary.
            var initial = await prov.GetAsync();
            await TryRegisterAsync(gw, log, initial);

            // Re-register on every change.
            cache.Changed += (_, s) =>
            {
                _ = Task.Run(async () =>
                {
                    using var innerScope = app.Services.CreateScope();
                    var gw2 = innerScope.ServiceProvider.GetRequiredService<ISmsGatewayClient>();
                    await TryRegisterAsync(gw2, log, s);
                });
            };
        });
    });

    return app;
}


    private static async Task TryRegisterAsync(
        ISmsGatewayClient gw, ILogger log, SmsGatewaySettingsDto s)
    {
        if (string.IsNullOrWhiteSpace(s.WebhookPublicUrl))
        {
            log.LogDebug("SMS webhook URL empty; skipping registration.");
            return;
        }

        try
        {
            await gw.SetWebhookAsync(s.WebhookPublicUrl, s.WebhookSecret);
            log.LogInformation("SMS webhook registered: {Url}", s.WebhookPublicUrl);
        }
        catch (Exception ex)
        {
            log.LogWarning(ex,
                "Could not register SMS webhook at {Url} (phone offline?)",
                s.WebhookPublicUrl);
        }
    }

}