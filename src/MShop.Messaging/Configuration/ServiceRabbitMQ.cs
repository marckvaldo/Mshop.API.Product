using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace MShop.Messaging.Configuration
{
    public class ServiceRabbitMQ
    {
        private readonly IOptions<RabbitMQConfiguration> _rabbitmqConfiguration;
        private readonly IModel _channel;

        private readonly string _exchenge;
        private readonly string _nameQueue;
        private readonly string[] _routeKey = { "product.#", "category.#" };
        //private readonly string _routeKeyCategory = "category.#";
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
            foreach (var routeKey in _routeKey)
            {
                _channel.QueueBind(queueDead!, exchengeDead!, routeKey, null);
            }

            return new Dictionary<string, object>
            {
                {"x-dead-letter-exchange",exchengeDead}
            };

        }

        public void SetUp()
        {
            _channel.ExchangeDeclare(_exchenge, "topic", _durable, false, null);
            _channel.QueueDeclare(_nameQueue, _durable, false, false);
            foreach(var routeKey in _routeKey)
            {
                _channel.QueueBind(_nameQueue, _exchenge, routeKey, null);
            }
            

        }

        public void SetUpWithDeadLetter()
        {
            var arguments = DeadLertterQueue();

            _channel.ExchangeDeclare(_exchenge, "topic", _durable, false, null);
            _channel.QueueDeclare(_nameQueue, _durable, false, false, arguments);

            foreach (var routeKey in _routeKey)
            {
                _channel.QueueBind(_nameQueue, _exchenge, routeKey, null);
            }
                
        }
        public void Down()
        {
            foreach (var routeKey in _routeKey)
            {
                _channel.QueueUnbind(_nameQueue, _exchenge, routeKey, null);
            }

            _channel.QueueDelete(_nameQueue, false, false);
            _channel.ExchangeDelete(_exchenge, false);
        }

        public void DownDeadLetter()
        {
            var exchengeDead = $"{_exchenge}.DeadLetter";
            var queueDead = $"{_nameQueue}.DeadLetter";

            foreach (var routeKey in _routeKey)
            {
                _channel.QueueUnbind(queueDead, exchengeDead, routeKey, null);
            }
            _channel.QueueDelete(queueDead, false, false);
            _channel.ExchangeDelete(exchengeDead, false);
        }
    }
}
