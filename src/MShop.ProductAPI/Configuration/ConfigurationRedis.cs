namespace MShop.ProductAPI.Configuration
{
    public static class ConfigurationRedis
    {
        public static IServiceCollection AddConfigurationRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var redisPassword = configuration["Redis:Password"];
            var redisEndPoint = configuration["Redis:Endpoint"];

            services.AddStackExchangeRedisCache(options =>
            {
                options.InstanceName = "Redis";
                options.Configuration = redisEndPoint;
                if (!string.IsNullOrEmpty(redisPassword))
                {
                    options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions { Password = redisPassword };
                    options.ConfigurationOptions.EndPoints.Add(redisPassword);
                }
                

            });

            return services;
        }
    }
}
