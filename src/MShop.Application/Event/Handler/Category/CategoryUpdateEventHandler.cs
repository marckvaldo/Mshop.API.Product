using Elasticsearch.Net;
using MShop.Business.Events.Category;
using MShop.Core.Message.DomainEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Application.Event.Handler.Category
{
    public class CategoryUpdateEventHandler : IDomainEventHandler<CategoryUpdateEvent>
    {
        private IMessageProducer _messageProducer;

        public CategoryUpdateEventHandler(IMessageProducer messageProducer)
        {
            _messageProducer = messageProducer;
        }

        public async Task HandlerAsync(CategoryUpdateEvent domainEvent)
        {
            if (domainEvent.Id == Guid.Empty) return;
            
           await _messageProducer.SendMessageAsync(domainEvent);
        }
    }
}
