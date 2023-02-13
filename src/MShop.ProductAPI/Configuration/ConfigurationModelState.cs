using Microsoft.AspNetCore.Mvc;

namespace MShop.ProductAPI.Configuration
{
    public static class ConfigurationModelState
    {
        public static IServiceCollection AddConfigurationModelState(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            return services;
        }
    }
}
