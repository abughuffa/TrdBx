using CleanArchitecture.Blazor.Application.Common.Interfaces;
using CleanArchitecture.Blazor.Infrastructure.Configurations;
using CleanArchitecture.Blazor.Infrastructure.Constants.Database;
using CleanArchitecture.Blazor.Infrastructure.Services.BackupRestoreStrategies;
using CleanArchitecture.Blazor.Infrastructure.Services.Wialon;
using CleanArchitecture.Blazor.Infrastructure.Services.Wialon.Implementation;
using Microsoft.Extensions.Configuration;

namespace CleanArchitecture.Blazor.Infrastructure;
public static partial class DependencyInjection
{
    private const string WIALON_ADDRESS_KEY = "Wialon:BaseUrl";


    public static IServiceCollection AddTrdBxInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddTrDbxServices(configuration);
    }


    private static IServiceCollection AddTrDbxServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<WialonSettings>(configuration.GetSection("WialonSettings"));

        services.AddHttpClient<IWialonSession, WialonSession>(client =>
        {
            client.BaseAddress = new Uri(configuration["WialonSettings:BaseUrl"]!);
        });

        services.AddScoped<IWialonWrapper, WialonWrapper>();
        services.AddHealthChecks().AddCheck<WialonHealthCheck>("Wialon");
        services.AddScoped<IWialonService, WialonService>();
        //services.AddScoped<IPermissionService, PermissionService>();
        services.AddBackupRestoreServices(configuration);
        return services;
    }


    private static IServiceCollection AddBackupRestoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseSettings = configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>();

        // Register the appropriate strategy based on DBProvider
        switch (databaseSettings.DBProvider.ToLowerInvariant())
        {
            case DbProviderKeys.SqLite:
                services.AddScoped<IDatabaseBackupRestoreStrategy, SqliteBackupRestoreStrategy>();
                break;
            case DbProviderKeys.SqlServer:
                services.AddScoped<IDatabaseBackupRestoreStrategy, SqlServerBackupRestoreStrategy>();
                break;
            //case DbProviderKeys.Npgsql:
            //    services.AddScoped<IDatabaseBackupRestoreStrategy, PostgresBackupRestoreStrategy>();
            //    break;
            default:
                throw new InvalidOperationException($"DB Provider {databaseSettings.DBProvider} is not supported.");
        }

        services.AddScoped<IBackupRestoreService, BackupRestoreService>();

        return services;
    }





 
}
