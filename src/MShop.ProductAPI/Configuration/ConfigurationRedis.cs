namespace MShop.ProductAPI.Configuration
{
    public static class ConfigurationRedis
    {
        public static IServiceCollection AddConfigurationRedis(this IServiceCollection services, string redisPassword,string redisEndPoint)
        {
            //var configuracao = builder.Configuration;
            //var redisPassword = configuracao["Redis:Password"];
            //var redisEndPoint = configuracao["Redis:Endpoint"];*/

            services.AddStackExchangeRedisCache(options =>
            {
                options.InstanceName = "Redis";
                options.Configuration = redisEndPoint;
                options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions { Password = redisPassword };
                options.ConfigurationOptions.EndPoints.Add(redisPassword);

            });

            return services;
        }
    }
}
