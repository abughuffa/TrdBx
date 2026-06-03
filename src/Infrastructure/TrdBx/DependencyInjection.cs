using CleanArchitecture.Blazor.Domain;
using CleanArchitecture.Blazor.Infrastructure.Configurations;
using CleanArchitecture.Blazor.Infrastructure.Constants.Database;
using CleanArchitecture.Blazor.Infrastructure.Services.RestoreBackupStrategies;
using Microsoft.Extensions.Configuration;
namespace CleanArchitecture.Blazor.Infrastructure;

/// <summary>
/// Dependency injection configuration for infrastructure services
/// </summary>
public static partial class DependencyInjection
{

    /// <summary>
    /// Adds all infrastructure services to the DI container
    /// </summary>
    public static IServiceCollection AddTrdBxInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddWialonServices(configuration);
        services.AddBackupRestoreServices(configuration);
        return services;
    }

    /// <summary>
    /// Adds Wialon-specific services to the DI container
    /// </summary>
    public static IServiceCollection AddWialonServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Validate configuration
        var wialonConfig = configuration.GetSection("Wialon").Get<WialonSessionConfig>();
        if (wialonConfig == null)
        {
            throw new InvalidOperationException("Wialon configuration section is missing from appsettings.json");
        }
        
        if (string.IsNullOrWhiteSpace(wialonConfig.Token))
        {
            throw new InvalidOperationException(
                "Wialon token is not configured. Please set it in appsettings.json, user secrets, or environment variables.");
        }

        // Register configuration for IOptions injection
        services.Configure<WialonSessionConfig>(configuration.GetSection("Wialon"));
        
        // Register typed HttpClient for Wialon service
        // This will automatically register IWialonService with the correct HttpClient
        services.AddHttpClient<IWialonService, WialonService>((serviceProvider, client) =>
        {
            var config = configuration.GetSection("Wialon").Get<WialonSessionConfig>();
            client.BaseAddress = new Uri(config?.BaseUrl ?? "https://cms.eagleeye.ly");
            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Add("User-Agent", "BlazorServer-WialonClient/1.0");
        });
        
        // Register background service for automatic session management
        services.AddHostedService<WialonSessionBackgroundService>();

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
            case DbProviderKeys.Npgsql:
                services.AddScoped<IDatabaseBackupRestoreStrategy, PostgresBackupRestoreStrategy>();
                break;
            default:
                throw new InvalidOperationException($"DB Provider {databaseSettings.DBProvider} is not supported.");
        }

        services.AddScoped<IBackupRestoreService, BackupRestoreService>();

        return services;
    }
}
