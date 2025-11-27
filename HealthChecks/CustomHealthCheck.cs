using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DotnetConcepts.HealthChecks;

public class CustomHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default
    )
    {
        return Task.FromResult(new HealthCheckResult(HealthStatus.Unhealthy));
    }
}
