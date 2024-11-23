using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace MShop.Messaging.Configuration
{
    public class ServiceRabbitMQ
    {
        private readonly IOptions<RabbitMQConfiguration> _rabbitmqConfiguration;
        private readonly IModel _channel;

        private readonly string _exchengeProduct;
        private readonly string _nameQueueProduct;
        private readonly string[] _routeKey = { "product.#", "category.#" };
        //private readonly string _routeKeyCategory = "category.#";
        private readonly bool _durable;

        public ServiceRabbitMQ(IOptions<RabbitMQConfiguration> rabbitmqConfiguration, IModel channel)
        {
            _rabbitmqConfiguration = rabbitmqConfiguration;
            _channel = channel;
            _exchengeProduct = _rabbitmqConfiguration.Value.Exchange;
            _nameQueueProduct = _rabbitmqConfiguration.Value.QueueProducts;
            _durable = rabbitmqConfiguration.Value.Durable;
            //_routeKey = "";
        }


        private Dictionary<string, object> DeadLertterQueue()
        {
            var exchengeDead = $"{_exchengeProduct}.DeadLetter";
            var queueDead = $"{_nameQueueProduct}.DeadLetter";

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
            _channel.ExchangeDeclare(_exchengeProduct, "topic", _durable, false, null);
            _channel.QueueDeclare(_nameQueueProduct, _durable, false, false);
            foreach(var routeKey in _routeKey)
            {
                _channel.QueueBind(_nameQueueProduct, _exchengeProduct, routeKey, null);
            }            

        }

        public void SetUpWithDeadLetter()
        {
            var arguments = DeadLertterQueue();

            _channel.ExchangeDeclare(_exchengeProduct, "topic", _durable, false, null);
            _channel.QueueDeclare(_nameQueueProduct, _durable, false, false, arguments);

            foreach (var routeKey in _routeKey)
            {
                _channel.QueueBind(_nameQueueProduct, _exchengeProduct, routeKey, null);
            }
                
        }
        public void Down()
        {
            foreach (var routeKey in _routeKey)
            {
                _channel.QueueUnbind(_nameQueueProduct, _exchengeProduct, routeKey, null);
            }

            _channel.QueueDelete(_nameQueueProduct, false, false);
            _channel.ExchangeDelete(_exchengeProduct, false);
        }

        public void DownDeadLetter()
        {
            var exchengeDead = $"{_exchengeProduct}.DeadLetter";
            var queueDead = $"{_nameQueueProduct}.DeadLetter";

            foreach (var routeKey in _routeKey)
            {
                _channel.QueueUnbind(queueDead, exchengeDead, routeKey, null);
            }
            _channel.QueueDelete(queueDead, false, false);
            _channel.ExchangeDelete(exchengeDead, false);
        }
    }
}
