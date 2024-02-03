using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace TodoApi.Middlewares;

public class HealthCheckHandler : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(HealthCheckResult.Healthy());
    }
}
