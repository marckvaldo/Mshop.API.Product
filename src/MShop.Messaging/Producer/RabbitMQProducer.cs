using Microsoft.Extensions.Options;
using MShop.Core.Message.DomainEvent;
using MShop.Messaging.Configuration;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Channels;

namespace MShop.Messaging.Producer
{
    public class RabbitMQProducer : IMessageProducer
    {
        private readonly IModel _channel;
        private readonly string _exchenge;
        //private readonly string _nameQueueProduct;
        //private readonly string _routeKey = "product.#";

        public RabbitMQProducer(IModel channel, IOptions<RabbitMQConfiguration> options)
        {
            _channel = channel;
            _exchenge = options.Value.Exchange;
            //_nameQueueProduct = options.Value.QueueProducts;

            //informar que deseja uma confirmacao
            _channel.ConfirmSelect();

            //_channel.ExchangeDeclare(_exchengeProduct, "topic", true, false, null);
            //_channel.QueueDeclare(_nameQueueProduct, true, false, false);
            //_channel.QueueBind(_nameQueueProduct, _exchengeProduct, _routeKey, null);
        }

        public Task SendMessageAsync<T>(T message)
        {
            var routingKey = EventsMapping.GetRoutingKey<T>();
            var messageBytes = JsonSerializer.SerializeToUtf8Bytes(message, 
                new JsonSerializerOptions { 
                    ReferenceHandler = ReferenceHandler.IgnoreCycles
                });

            var basicProperties = _channel.CreateBasicProperties();
            basicProperties.Type = typeof(T).Name;  // Definindo o valor do Type
            basicProperties.ContentType = "application/json";


            _channel.BasicPublish(
                exchange: _exchenge,
                routingKey: routingKey,
                body: messageBytes,
                basicProperties: basicProperties,
                mandatory: true); // aqui ele fica olhando se a messagem chegou na fila não apenas na exchenger

            //aqui ele fica esperando por 5 segundos a respostas
            //_channel.WaitForConfirms(new TimeSpan(0, 0, 5));

            //informa que o sistema espera uma confirmação.
            _channel.WaitForConfirmsOrDie();
            return Task.CompletedTask;
        }
    }
}
