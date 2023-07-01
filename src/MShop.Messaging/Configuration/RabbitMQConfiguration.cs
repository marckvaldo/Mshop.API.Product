using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Messaging.Configuration
{
    public class RabbitMQConfiguration
    {
        public const string ConfigurationSection = "RabbitMQ";
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Exchange { get; set; }   
        public int Port { get; set; }
    }
}
