﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MShop.Application.Event;
using MShop.Application.Event.Handler.Products;
using MShop.Business.Events.Products;
using MShop.Business.Interface.Event;
using MShop.Messaging.Configuration;
using MShop.Messaging.Producer;
using RabbitMQ.Client;

namespace MShop.ProductAPI.Configuration
{
    public static class ConfigurationEvents
    {
        public static IServiceCollection AddConfigurationEvents(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDomainEventPublisher, DomainEventPublisher>();

            services.AddTransient<IDomainEventHandler<ProductCreatedEvent>, ProductCreatedEventHandler>();
            services.AddTransient<IDomainEventHandler<ProductUpdatedEvent>, ProductUpdatedEventHandler>();
            services.AddTransient<IDomainEventHandler<ProductRemovedEvent>, ProductRemovedEventHandler>();

            //coloca as configurações RabbitMQ nas conficurações da minha applicação
            services.Configure<RabbitMQConfiguration>(configuration.GetSection(RabbitMQConfiguration.ConfigurationSection));

            //faz a connection com rabbitmq
            services.AddSingleton(options =>
            {
                RabbitMQConfiguration config = options.GetRequiredService<IOptions<RabbitMQConfiguration>>().Value;

                var factory = new ConnectionFactory
                {
                    HostName = config.HostName,
                    UserName = config.UserName,
                    Password = config.Password,
                };

                return factory.CreateConnection();
            });

            services.AddSingleton<ChannelManager>();

            services.AddTransient<IMessageProducer>(options =>
            {
                //aqui eu chamo a manager channel
                var channelManager = options.GetRequiredService<ChannelManager>();
                var config = options.GetRequiredService<IOptions<RabbitMQConfiguration>>();

                return new RabbitMQProducer(channelManager.GetChannel(), config);
            });

            return services;
        }
    }
}