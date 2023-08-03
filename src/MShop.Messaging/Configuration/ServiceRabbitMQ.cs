using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace MShop.Messaging.Configuration
{
    public class ServiceRabbitMQ
    {
        private readonly IOptions<RabbitMQConfiguration> _rabbitmqConfiguration;
        private readonly IModel _channel;

        private readonly string _exchenge;
        private readonly string _nameQueue;
        private readonly string _routeKey = "product.#";

        public ServiceRabbitMQ(IOptions<RabbitMQConfiguration> rabbitmqConfiguration, IModel channel)
        {
            _rabbitmqConfiguration = rabbitmqConfiguration;
            _channel = channel;
            _exchenge = _rabbitmqConfiguration.Value.Exchange;
            _nameQueue = _rabbitmqConfiguration.Value.QueueProducts;
            //_routeKey = "";
        }

        public void SetUp()
        {
            _channel.ExchangeDeclare(_exchenge, "topic", true, false, null);
            _channel.QueueDeclare(_nameQueue, true, false, false);
            _channel.QueueBind(_nameQueue, _exchenge, _routeKey, null);
        }

        public void Down()
        {
            _channel.QueueUnbind(_nameQueue, _exchenge, _routeKey, null);
            _channel.QueueDelete(_nameQueue, false, false);
            _channel.ExchangeDelete(_exchenge, false);
        }
    }
}
