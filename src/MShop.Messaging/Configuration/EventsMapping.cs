using MShop.Business.Events.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Messaging.Configuration
{
    public class EventsMapping
    {
        private static Dictionary<string, string> _routingKey => new()
        {
            {typeof(ProductCreatedEvent).Name, "video.Event" },
            {typeof(ProductUpdatedEvent).Name, "video.Event" },
            {typeof(ProductRemovedEvent).Name, "video.Event" }
        };

        public static string GetRoutingKey<T>() => _routingKey[typeof(T).Name];

    }
}
