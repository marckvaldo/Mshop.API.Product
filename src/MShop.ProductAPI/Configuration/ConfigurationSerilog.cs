using Serilog;

namespace MShop.ProductAPI.Configuration
{
    public static class ConfigurationSerilog
    {
        public static IServiceCollection AddConfigurationSeriLog(this IServiceCollection services, IConfiguration configuration)
        {
            var elasticUri = configuration["Elasticsearch:Uri"];
            var elasticUsername = configuration["Elasticsearch:Username"];
            var elasticPassword = configuration["Elasticsearch:Password"];

            var ElasticsearchConfiguration = new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri($"http://{elasticUsername}:{elasticPassword}@{elasticUri}"))
            {
                AutoRegisterTemplate = true,
                IndexFormat = "logaplication-{0:yyyy.MM.dd}",
            };

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .WriteTo.Elasticsearch(ElasticsearchConfiguration)
                .CreateBootstrapLogger();

            return services;

        }
    }
}
