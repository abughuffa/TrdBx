using CleanArchitecture.Blazor.Application.Features.SmsGatewaySettings.DTOs;

namespace CleanArchitecture.Blazor.Application.Common.Interfaces;

/// <summary>
/// Scoped provider that reads/writes the DB-backed SMS gateway settings
/// and publishes updates to the singleton <see cref="ISmsGatewaySettingsCache"/>.
/// </summary>
public interface ISmsGatewaySettingsProvider
{
    Task<SmsGatewaySettingsDto> GetAsync(CancellationToken ct = default);
    Task UpdateAsync(SmsGatewaySettingsDto settings, CancellationToken ct = default);
}