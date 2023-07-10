using Microsoft.Extensions.Options;
using MShop.Business.Interface.Event;
using MShop.Messaging.Configuration;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MShop.Messaging.Producer
{
    public class RabbitMQProducer : IMessageProducer
    {
        private readonly IModel _channel;
        private readonly string _exchenge;

        public RabbitMQProducer(IModel channel, IOptions<RabbitMQConfiguration> options)
        {
            _channel = channel;
            _exchenge = options.Value.Exchange;
            
        }

        public Task SendMessageAsync<T>(T message)
        {
            var routingKey = EventsMapping.GetRoutingKey<T>();
            var messageBytes = JsonSerializer.SerializeToUtf8Bytes(message, 
                new JsonSerializerOptions { 
                    ReferenceHandler = ReferenceHandler.IgnoreCycles
                });

            //informar que deseja uma confirmacao
            _channel.ConfirmSelect();

            _channel.BasicPublish(
                exchange: _exchenge,
                routingKey: routingKey,
                body: messageBytes);
            //informa que o sistema espera uma confirmação.
            _channel.WaitForConfirmsOrDie();
            return Task.CompletedTask;
        }
    }
}
