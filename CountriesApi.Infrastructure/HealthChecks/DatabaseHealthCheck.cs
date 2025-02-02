using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CountriesApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CountriesApi.Infrastructure.HealthChecks
{
    public class DatabaseHealthCheck(AppDbContext dbContext) : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
        {
            try
            {
                // Use a simple query with timeout
                await dbContext.Database.ExecuteSqlRawAsync(
                    sql: "SELECT 1",
                    cancellationToken: cancellationToken
                ).WaitAsync(TimeSpan.FromSeconds(3));

                return HealthCheckResult.Healthy("Database is available.");
            }
            catch (TimeoutException ex)
            {
                return HealthCheckResult.Unhealthy("Database connection timed out.", ex);
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Database is unavailable.", ex);
            }
        }
    }
}
