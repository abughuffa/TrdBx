using CleanArchitecture.Blazor.Application.Common.Interfaces;
using CleanArchitecture.Blazor.Application.Features.SmsGatewaySettings.DTOs;
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
        public static IServiceCollection AddSmsGatewayServices(
        this IServiceCollection services, IConfiguration config)
    {
        services.AddHostedService<SmsInboundPoller>();
        return services;
    }

    public static WebApplication MapSmsGatewayEndpoints(this WebApplication app)
    {
        app.MapSmsWebhook();
        return app;
    }

    // public static WebApplication RegisterSmsWebhookOnStartup(this WebApplication app)
    // {
    //     // Register once at startup, then re-register on every settings change.
    //     app.Lifetime.ApplicationStarted.Register(() =>
    //     {
    //         _ = Task.Run(async () => await RegisterAndWatchAsync(app));
    //     });

    //     return app;
    // }

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

    private static async Task RegisterAndWatchAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var sp       = scope.ServiceProvider;

        var provider = sp.GetRequiredService<ISmsGatewaySettingsProvider>();
var cache    = sp.GetRequiredService<ISmsGatewaySettingsCache>();
var gw       = sp.GetRequiredService<ISmsGatewayClient>();
var log      = sp.GetRequiredService<ILogger<Program>>();

// Initial registration — triggers the DB read + seeds the cache.
var initial = await provider.GetAsync();
await TryRegisterAsync(gw, log, initial);

// Subscribe to change events on the singleton cache.
cache.Changed += (_, s) =>
{
    _ = Task.Run(async () =>
    {
        using var inner = app.Services.CreateScope();
        var gw2 = inner.ServiceProvider.GetRequiredService<ISmsGatewayClient>();
        await TryRegisterAsync(gw2, log, s);
    });
};

        // var settings = sp.GetRequiredService<ISmsGatewaySettingsProvider>();
        // var gw       = sp.GetRequiredService<ISmsGatewayClient>();
        // var log      = sp.GetRequiredService<ILogger<Program>>();


        // // Initial registration.
        // await TryRegisterAsync(gw, log, await settings.GetAsync());

//         var settings = sp.GetRequiredService<ISmsGatewaySettingsProvider>();
// var gw       = sp.GetRequiredService<ISmsGatewayClient>();
// var log      = sp.GetRequiredService<ILogger<Program>>();

// await TryRegisterAsync(gw, log, await settings.GetAsync());

//         // Re-register when settings change.
//         settings.Changed += (_, s) =>            // ← error: no such member
// {
//     _ = Task.Run(async () =>
//     {
//         using var inner = app.Services.CreateScope();
//         var gw2 = inner.ServiceProvider.GetRequiredService<ISmsGatewayClient>();
//         await TryRegisterAsync(gw2, log, s);
//     });

        // settings.Changed += (_, s) =>
        // {
        //     _ = Task.Run(async () =>
        //     {
        //         using var s2 = app.Services.CreateScope();
        //         var gw2 = s2.ServiceProvider.GetRequiredService<ISmsGatewayClient>();
        //         await TryRegisterAsync(gw2, log, s);
        //     });
        // };
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

    
    // /// <summary>
    // /// Registers the SMS inbound poller. Call once during host building.
    // /// </summary>
    // public static IServiceCollection AddSmsGatewayServices(
    //     this IServiceCollection services,
    //     IConfiguration config)
    // {
    //     // The SMS section is optional; if the phone is not configured the
    //     // poller short-circuits based on EnableInboundPolling.
    //     services.AddHostedService<SmsInboundPoller>();
    //     return services;
    // }

    // /// <summary>
    // /// Maps the SMS webhook endpoint. Call after <c>app.Build()</c> and
    // /// before <c>app.Run()</c>.
    // /// </summary>
    // public static WebApplication MapSmsGatewayEndpoints(this WebApplication app)
    // {
    //     app.MapSmsWebhook();
    //     return app;
    // }

    // /// <summary>
    // /// Registers the webhook URL with the phone once the app has fully
    // /// started. Fire-and-forget; a failure is logged but never fatal.
    // /// Idempotent — safe to call multiple times.
    // /// </summary>
    // public static WebApplication RegisterSmsWebhookOnStartup(this WebApplication app)
    // {
    //     app.Lifetime.ApplicationStarted.Register(() =>
    //     {
    //         _ = Task.Run(async () =>
    //         {
    //             using var scope = app.Services.CreateScope();
    //             var sp  = scope.ServiceProvider;
    //             var gw  = sp.GetRequiredService<ISmsGatewayClient>();
    //             var cfg = sp.GetRequiredService<IConfiguration>();
    //             var log = sp.GetRequiredService<ILogger<Program>>();

    //             var url = cfg["SmsGateway:WebhookPublicUrl"];
    //             if (string.IsNullOrWhiteSpace(url))
    //             {
    //                 log.LogDebug("SmsGateway:WebhookPublicUrl is not set; skipping webhook registration.");
    //                 return;
    //             }

    //             try
    //             {
    //                 await gw.SetWebhookAsync(url, cfg["SmsGateway:WebhookSecret"]);
    //                 log.LogInformation("SMS webhook registered: {Url}", url);

    //                 // Optional startup reachability probe (uncomment to enable)
    //                 // try
    //                 // {
    //                 //     var s = await gw.GetStatusAsync();
    //                 //     log.LogInformation(
    //                 //         "SMS gateway reachable: mode={Mode} simReady={Sim} sent={Sent} received={Recv}",
    //                 //         s.Mode, s.SimReady, s.SentCount, s.ReceivedCount);
    //                 // }
    //                 // catch (Exception ex)
    //                 // {
    //                 //     log.LogWarning(ex, "SMS gateway not reachable at startup; poller will retry with backoff.");
    //                 // }
    //             }
    //             catch (Exception ex)
    //             {
    //                 log.LogWarning(ex, "Could not register SMS webhook (phone offline?)");
    //             }
    //         });
    //     });

    //     return app;
    // }
}