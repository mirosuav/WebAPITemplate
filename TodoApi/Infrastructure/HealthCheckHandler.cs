using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace TodoApi.Infrastructure;

public class HealthCheckHandler : IHealthCheck
{
    public  Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(HealthCheckResult.Healthy());
    }
}
