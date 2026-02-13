using CodeLab.HealthChecks.Api.Data;
using Dapper;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CodeLab.HealthChecks.Api.Health;

internal sealed class DatabaseHealthCheck(IDbConnectionFactory connectionFactory, ILogger<DatabaseHealthCheck> logger) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            using var connection = await connectionFactory.CreateConnectionAsync(cancellationToken);
            connection.ExecuteScalarAsync<bool>("SELECT 1", cancellationToken);
            
            logger.LogInformation("Database healthy");
            return HealthCheckResult.Healthy("Database healthy");
        }
        catch (Exception ex)
        {
            logger.LogError(ex,"The database is unhealthy");
            return  HealthCheckResult.Unhealthy(exception: ex, description: "The database is unhealthy");
        }
    }
}