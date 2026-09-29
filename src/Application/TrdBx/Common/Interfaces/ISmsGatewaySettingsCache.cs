using CleanArchitecture.Blazor.Application.Features.SmsGatewaySettings.DTOs;

namespace CleanArchitecture.Blazor.Application.Common.Interfaces;

/// <summary>
/// Singleton in-memory snapshot of the current SMS gateway settings.
/// Updated by <see cref="ISmsGatewaySettingsProvider"/> on every write,
/// and by the first read after startup.
/// </summary>
public interface ISmsGatewaySettingsCache
{
    /// <summary>Current snapshot, or null if never loaded.</summary>
    SmsGatewaySettingsDto? Current { get; }

    /// <summary>Replace the snapshot and raise <see cref="Changed"/>.</summary>
    void Set(SmsGatewaySettingsDto settings);

    /// <summary>Fires after a successful update.</summary>
    event EventHandler<SmsGatewaySettingsDto>? Changed;
}