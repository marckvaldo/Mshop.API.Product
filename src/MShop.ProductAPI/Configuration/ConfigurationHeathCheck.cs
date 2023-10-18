using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MShop.Application.Health;
using MShop.Repository.Context;

namespace MShop.ProductAPI.Configuration
{
    public static class ConfigurationHeathCheck
    {
        public static IServiceCollection AddConfigurationHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddCheck<HealthDataBase>("DataBase")
                .AddCheck<HealthMessagingBroker>("MessagingBroker");

            return services;
        }


        public static WebApplication AddMapHealthCheck(this WebApplication app)
        {
            app.MapHealthChecks("/_metrics", new HealthCheckOptions
            {
                ResponseWriter =UIResponseWriter.WriteHealthCheckUIResponse 
            });
            return app;
        }
    }
}
