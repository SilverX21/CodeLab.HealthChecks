using Dapper;

namespace CodeLab.HealthChecks.Api.Data;

internal sealed class DbInitializer(
    IDbConnectionFactory dbConnectionFactory,
    ILogger<DbInitializer> logger)
{
    public async Task InitializeAsync()
    {
        try
        {
            logger.LogInformation("Initializing database");

            using var connection = await dbConnectionFactory.CreateConnectionAsync();

            await connection.ExecuteAsync(
                """
                --create teams table if doesn't exist
                CREATE TABLE IF NOT EXISTS public.teams (
                    id UUID PRIMARY KEY,
                    name text NOT NULL,
                    city_name text NOT NULL,
                    foundation_date timestamp NOT NULL
                )
                """);

            logger.LogInformation("Database initialization completed!");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing the database");
        }
    }
}