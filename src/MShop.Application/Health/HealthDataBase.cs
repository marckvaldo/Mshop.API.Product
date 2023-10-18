using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MShop.Repository.Context;
using MySqlConnector;

namespace MShop.Application.Health
{
    public class HealthDataBase : IHealthCheck
    {
        protected readonly string _connectionString;       

        public HealthDataBase(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("RepositoryMysql");
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using (DbConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                        return HealthCheckResult.Healthy();
                    else
                        return HealthCheckResult.Unhealthy();

                   
                }
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new HealthCheckResult(
                         status: HealthStatus.Unhealthy,
                         description: ex.Message.ToString()
                         ));
            }
        }

        
    }
}
