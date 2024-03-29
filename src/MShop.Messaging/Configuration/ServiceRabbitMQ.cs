﻿using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace MShop.Messaging.Configuration
{
    public class ServiceRabbitMQ
    {
        private readonly IOptions<RabbitMQConfiguration> _rabbitmqConfiguration;
        private readonly IModel _channel;

        private readonly string _exchenge;
        private readonly string _nameQueue;
        private readonly string _routeKey = "product.#";
        private readonly bool _durable;

        public ServiceRabbitMQ(IOptions<RabbitMQConfiguration> rabbitmqConfiguration, IModel channel)
        {
            _rabbitmqConfiguration = rabbitmqConfiguration;
            _channel = channel;
            _exchenge = _rabbitmqConfiguration.Value.Exchange;
            _nameQueue = _rabbitmqConfiguration.Value.QueueProducts;
            _durable = rabbitmqConfiguration.Value.Durable;
            //_routeKey = "";
        }


        private Dictionary<string, object> DeadLertterQueue()
        {
            var exchengeDead = $"{_exchenge}.DeadLetter";
            var queueDead = $"{_nameQueue}.DeadLetter";

            _channel.ExchangeDeclare(exchengeDead!, "topic", _durable, false, null);
            _channel.QueueDeclare(queueDead!, _durable, false, false);
            _channel.QueueBind(queueDead!, exchengeDead!, _routeKey, null);

            return new Dictionary<string, object>
            {
                {"x-dead-letter-exchange",exchengeDead}
            };

        }

        public void SetUp()
        {
            _channel.ExchangeDeclare(_exchenge, "topic", _durable, false, null);
            _channel.QueueDeclare(_nameQueue, _durable, false, false);
            _channel.QueueBind(_nameQueue, _exchenge, _routeKey, null);
        }

        public void SetUpWithDeadLetter()
        {
            var arguments = DeadLertterQueue();

            _channel.ExchangeDeclare(_exchenge, "topic", _durable, false, null);
            _channel.QueueDeclare(_nameQueue, _durable, false, false, arguments);
            _channel.QueueBind(_nameQueue, _exchenge, _routeKey, null);
        }
        public void Down()
        {
            _channel.QueueUnbind(_nameQueue, _exchenge, _routeKey, null);
            _channel.QueueDelete(_nameQueue, false, false);
            _channel.ExchangeDelete(_exchenge, false);
        }

        public void DownDeadLetter()
        {
            var exchengeDead = $"{_exchenge}.DeadLetter";
            var queueDead = $"{_nameQueue}.DeadLetter";

            _channel.QueueUnbind(queueDead, exchengeDead, _routeKey, null);
            _channel.QueueDelete(queueDead, false, false);
            _channel.ExchangeDelete(exchengeDead, false);
        }
    }
}
