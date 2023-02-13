using Microsoft.EntityFrameworkCore;
using MShop.Repository.Context;

namespace MShop.ProductAPI.Configuration
{
    public static class ConfigurationMySql
    {
        public static IServiceCollection AddConfigurationMySql(this IServiceCollection services, string ConnectionString)
        {
            services.AddDbContext<RepositoryDbContext>(options =>
                options.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString)));

            return services;
        }
    }
}
