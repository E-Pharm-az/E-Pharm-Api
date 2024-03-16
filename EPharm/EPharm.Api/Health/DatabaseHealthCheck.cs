using System.Data;
using EPharm.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace EPharmApi.Health;

public class DatabaseHealthCheck(AppDbContext appDbContext, AppIdentityDbContext appIdentityDbContext) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
    {
        try
        {
            var appDbConnection = appDbContext.Database.GetDbConnection();
            var appIdentityDbConnection = appIdentityDbContext.Database.GetDbConnection();

            await appDbConnection.OpenAsync(cancellationToken);
            await appIdentityDbConnection.OpenAsync(cancellationToken);
            
            if (appDbConnection.State == ConnectionState.Open && appIdentityDbConnection.State == ConnectionState.Open)
            {
                return HealthCheckResult.Healthy("Database is healthy.");
            }

            return HealthCheckResult.Unhealthy("Database is unhealthy.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return HealthCheckResult.Unhealthy("Database is unhealthy.", exception: ex);
        }
    }
}
