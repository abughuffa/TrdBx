using CleanArchitecture.Blazor.Application.Common.Interfaces;
using CleanArchitecture.Blazor.Server.UI.Endpoints;
using CleanArchitecture.Blazor.Server.UI.Hubs;
using CleanArchitecture.Blazor.Server.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Blazor.Server.UI;

/// <summary>
/// SMS Gateway wiring: hosted poller, webhook endpoint, and best-effort
/// webhook registration on startup. Split out from the main
/// DependencyInjection partial so SMS concerns stay isolated.
/// </summary>
public static partial class DependencyInjection
{
    /// <summary>
    /// Registers the SMS inbound poller. Call once during host building.
    /// </summary>
    public static IServiceCollection AddSmsGatewayServices(
        this IServiceCollection services,
        IConfiguration config)
    {
        // The SMS section is optional; if the phone is not configured the
        // poller short-circuits based on EnableInboundPolling.
        services.AddHostedService<SmsInboundPoller>();
        return services;
    }

    /// <summary>
    /// Maps the SMS webhook endpoint. Call after <c>app.Build()</c> and
    /// before <c>app.Run()</c>.
    /// </summary>
    public static WebApplication MapSmsGatewayEndpoints(this WebApplication app)
    {
        app.MapSmsWebhook();
        return app;
    }

    /// <summary>
    /// Registers the webhook URL with the phone once the app has fully
    /// started. Fire-and-forget; a failure is logged but never fatal.
    /// Idempotent — safe to call multiple times.
    /// </summary>
    public static WebApplication RegisterSmsWebhookOnStartup(this WebApplication app)
    {
        app.Lifetime.ApplicationStarted.Register(() =>
        {
            _ = Task.Run(async () =>
            {
                using var scope = app.Services.CreateScope();
                var sp  = scope.ServiceProvider;
                var gw  = sp.GetRequiredService<ISmsGatewayClient>();
                var cfg = sp.GetRequiredService<IConfiguration>();
                var log = sp.GetRequiredService<ILogger<Program>>();

                var url = cfg["SmsGateway:WebhookPublicUrl"];
                if (string.IsNullOrWhiteSpace(url))
                {
                    log.LogDebug("SmsGateway:WebhookPublicUrl is not set; skipping webhook registration.");
                    return;
                }

                try
                {
                    await gw.SetWebhookAsync(url, cfg["SmsGateway:WebhookSecret"]);
                    log.LogInformation("SMS webhook registered: {Url}", url);

                    // Optional startup reachability probe (uncomment to enable)
                    // try
                    // {
                    //     var s = await gw.GetStatusAsync();
                    //     log.LogInformation(
                    //         "SMS gateway reachable: mode={Mode} simReady={Sim} sent={Sent} received={Recv}",
                    //         s.Mode, s.SimReady, s.SentCount, s.ReceivedCount);
                    // }
                    // catch (Exception ex)
                    // {
                    //     log.LogWarning(ex, "SMS gateway not reachable at startup; poller will retry with backoff.");
                    // }
                }
                catch (Exception ex)
                {
                    log.LogWarning(ex, "Could not register SMS webhook (phone offline?)");
                }
            });
        });

        return app;
    }
}