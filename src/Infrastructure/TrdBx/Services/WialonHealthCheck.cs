using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CleanArchitecture.Blazor.Infrastructure.Services;
public class WialonHealthCheck : IHealthCheck
{
    private readonly IWialonWrapper _wialon;
    public WialonHealthCheck(IWialonWrapper wialon) => _wialon = wialon;

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken ct = default)
    {
        return await _wialon.TryConnect()
            ? HealthCheckResult.Healthy()
            : HealthCheckResult.Unhealthy();
    }
}
