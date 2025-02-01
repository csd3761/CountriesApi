using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CountriesApi.Infrastructure.Data;
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
                await dbContext.Database.CanConnectAsync(cancellationToken);
                return HealthCheckResult.Healthy("Database is available.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Database is unavailable.", ex);
            }
        }
    }
}
