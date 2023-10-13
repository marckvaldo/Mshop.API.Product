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
        //private readonly string _nameQueue;
        //private readonly string _routeKey = "product.#";

        public RabbitMQProducer(IModel channel, IOptions<RabbitMQConfiguration> options)
        {
            _channel = channel;
            _exchenge = options.Value.Exchange;
            //_nameQueue = options.Value.QueueProducts;

            //informar que deseja uma confirmacao
            _channel.ConfirmSelect();

            //_channel.ExchangeDeclare(_exchenge, "topic", true, false, null);
            //_channel.QueueDeclare(_nameQueue, true, false, false);
            //_channel.QueueBind(_nameQueue, _exchenge, _routeKey, null);
        }

        public Task SendMessageAsync<T>(T message)
        {
            var routingKey = EventsMapping.GetRoutingKey<T>();
            var messageBytes = JsonSerializer.SerializeToUtf8Bytes(message, 
                new JsonSerializerOptions { 
                    ReferenceHandler = ReferenceHandler.IgnoreCycles
                });

            _channel.BasicPublish(
                exchange: _exchenge,
                routingKey: routingKey,
                body: messageBytes,
                mandatory: true); // aqui ele fica olhando se a messagem chegou na fila não apenas na exchenger

            //aqui ele fica esperando por 5 segundos a respostas
            //_channel.WaitForConfirms(new TimeSpan(0, 0, 5));

            //informa que o sistema espera uma confirmação.
            _channel.WaitForConfirmsOrDie();
            return Task.CompletedTask;
        }
    }
}
