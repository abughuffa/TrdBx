using CleanArchitecture.Blazor.Application;
using CleanArchitecture.Blazor.Infrastructure;
using CleanArchitecture.Blazor.Infrastructure.Extensions;
using CleanArchitecture.Blazor.Server.UI;



var builder = WebApplication.CreateBuilder(args);
builder.RegisterSerilog();
builder.WebHost.UseStaticWebAssets();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddServerUI(builder.Configuration)

    .AddSmsGatewayServices();

var app = builder.Build();

// 2. SMS: map the webhook endpoint (must be after Build(), before ConfigureServer)
app.MapSmsGatewayEndpoints();

app.ConfigureServer(builder.Configuration);

await app.InitializeDatabaseAsync().ConfigureAwait(false);

// 3. SMS: best-effort webhook registration on startup (must be after Build())
app.RegisterSmsWebhookOnStartup();

await app.RunAsync().ConfigureAwait(false);
