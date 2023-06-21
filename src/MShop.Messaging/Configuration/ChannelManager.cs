using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Messaging.Configuration
{
    public class ChannelManager
    {
        private readonly IConnection _connection;
        private readonly object _lock = new ();
        private IModel _channel = null;
        public ChannelManager(IConnection connection) 
        {
            _connection = connection;
        }

        public IModel GetChannel() 
        {
            //evita concorrencia
            lock (_lock)
            {
                if (_channel == null || _channel.IsClosed)
                    _channel = _connection.CreateModel();

                return _channel;
            }
        }
    }
}
