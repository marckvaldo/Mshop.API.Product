using MShop.Business.Interface.Service;
using MShop.Business.Service;

namespace MShop.ProductAPI.Configuration
{
    public static class ConfigurationStorage
    {
        public static IServiceCollection AddConfigurationStorage(this IServiceCollection services)
        {
            services.AddTransient<IStorageService, StorageService>();
            return services;
        }
    }
}
