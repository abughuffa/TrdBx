using CleanArchitecture.Blazor.Application.Common.Interfaces;
using CleanArchitecture.Blazor.Application.Features.SmsGatewaySettings.DTOs;

namespace CleanArchitecture.Blazor.Infrastructure.TrdBx.Services;

public sealed class SmsGatewaySettingsCache : ISmsGatewaySettingsCache
{
    private SmsGatewaySettingsDto? _current;

    public SmsGatewaySettingsDto? Current => Volatile.Read(ref _current);

    public event EventHandler<SmsGatewaySettingsDto>? Changed;

    public void Set(SmsGatewaySettingsDto settings)
    {
        Volatile.Write(ref _current, settings);

        // Fire the event. Exceptions from subscribers must not break the caller.
        try { Changed?.Invoke(this, settings); }
        catch { /* swallow — logged by the provider */ }
    }
}