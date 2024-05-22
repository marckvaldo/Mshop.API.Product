using Microsoft.Extensions.Options;
using MShop.Application.Event;
using MShop.Application.Event.Handler.Products;
using MShop.Business.Events.Products;
using MShop.Core.Message.DomainEvent;
using MShop.Messaging.Configuration;
using MShop.Messaging.Producer;
using RabbitMQ.Client;

namespace MShop.ProductAPI.Configuration
{
    public static class ConfigurationEvents
    {
        public static IServiceCollection AddConfigurationEvents(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();

            services.AddScoped<IDomainEventHandler<ProductCreatedEvent>, ProductCreatedEventHandler>();
            services.AddScoped<IDomainEventHandler<ProductUpdatedEvent>, ProductUpdatedEventHandler>();
            services.AddScoped<IDomainEventHandler<ProductRemovedEvent>, ProductRemovedEventHandler>();

            //coloca as configurações RabbitMQ nos serviços da minha applicação para recuperar posteriomente
            services.Configure<RabbitMQConfiguration>(
                configuration.GetSection(RabbitMQConfiguration.ConfigurationSection)
                );

            //faz a connection com rabbitmq
            //toda fez vez que alguem chamar o IConnetion eu vou retornar essa conexão
            services.AddSingleton<IConnection>(options =>
            {
                RabbitMQConfiguration config = options.GetRequiredService<IOptions<RabbitMQConfiguration>>().Value;

                var factory = new ConnectionFactory
                {
                    HostName = config.HostName,
                    UserName = config.UserName,
                    Password = config.Password,
                    Port     = config.Port,
                    VirtualHost = config.Vhost
                };

                return factory.CreateConnection();
            });

            services.AddSingleton<ChannelManager>();

            //criar as queues 
            services.AddSingleton<ServiceRabbitMQ>(options =>
            {
                var channelManager = options.GetRequiredService<ChannelManager>();
                var config = options.GetRequiredService<IOptions<RabbitMQConfiguration>>();
                var serviceRabbitMQ = new ServiceRabbitMQ(config, channelManager.GetChannel());
                serviceRabbitMQ.SetUpWithDeadLetter();
                return serviceRabbitMQ;
            });

            services.AddScoped<IMessageProducer>(options =>
            {
                //aqui eu chamo a manager channel
                var channelManager = options.GetRequiredService<ChannelManager>();
                var config = options.GetRequiredService<IOptions<RabbitMQConfiguration>>();

                return new RabbitMQProducer(channelManager.GetChannel(), config);
            });

            return services;
        }

        public static WebApplication AddSetUpRabbiMQ(this WebApplication app)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (environment == "EndToEndTest") return app;

            using var scope = app.Services.CreateScope();
            var SetUpRabbiMQ = scope.ServiceProvider.GetRequiredService<ServiceRabbitMQ>();
            SetUpRabbiMQ.SetUpWithDeadLetter();

            return app;
        }
    }
}
