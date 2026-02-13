using CodeLab.HealthChecks.Api.Data;
using CodeLab.HealthChecks.Api.Health;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CodeLab.HealthChecks.Api.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabaseSettings(configuration);
        services.AddOpenApi();
        services.AddHealthChecksSettings(configuration);
        return services;
    }

    private static IServiceCollection AddDatabaseSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("HealthDb");
        services.AddSingleton<IDbConnectionFactory>(_ => new DbConnectionFactory(connectionString));
        services.AddSingleton<DbInitializer>();
        
        return services;
    }

    private static IServiceCollection AddHealthChecksSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("HealthDb");
        
        services.AddHealthChecks()
            .AddNpgSql(connectionString); //here is the health check for Postgres
        return services;
    }
}