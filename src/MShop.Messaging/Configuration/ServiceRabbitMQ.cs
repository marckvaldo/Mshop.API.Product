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
        private readonly string _routeKey = "product.#";

        public ServiceRabbitMQ(IOptions<RabbitMQConfiguration> rabbitmqConfiguration, IModel channel)
        {
            _rabbitmqConfiguration = rabbitmqConfiguration;
            _channel = channel;
            _exchenge = _rabbitmqConfiguration.Value.Exchange;
            _nameQueue = _rabbitmqConfiguration.Value.QueueProducts;
            //_routeKey = "";
        }


        private Dictionary<string, object> DeadLertterQueue()
        {
            var exchengeDead = $"{_exchenge}.DeadLetter";
            var queueDead = $"{_nameQueue}.DeadLetter";

            _channel.ExchangeDeclare(exchengeDead!, "topic", true, false, null);
            _channel.QueueDeclare(queueDead!, true, false, false);
            _channel.QueueBind(queueDead!, exchengeDead!, _routeKey, null);

            return new Dictionary<string, object>
            {
                {"x-dead-letter-exchange",exchengeDead}
            };

        }

        public void SetUp()
        {
            _channel.ExchangeDeclare(_exchenge, "topic", true, false, null);
            _channel.QueueDeclare(_nameQueue, true, false, false);
            _channel.QueueBind(_nameQueue, _exchenge, _routeKey, null);
        }

        public void SetUpWithDeadLetter()
        {
            var arguments = DeadLertterQueue();

            _channel.ExchangeDeclare(_exchenge, "topic", true, false, null);
            _channel.QueueDeclare(_nameQueue, true, false, false, arguments);
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
