﻿using MShop.Business.Events.Products;
using MShop.Business.Interface.Event;
using MShop.Business.Interface.Repository;

namespace MShop.Application.Event.Handler.Products
{
    public class SendToProductRemovedEventHandler : IDomainEventHandler<ProductRemovedEvent>
    {
        private readonly IMessageProducer _messageProducer;
        private readonly IProductRepository _productRepository;

        public SendToProductRemovedEventHandler(IMessageProducer messageProducer, IProductRepository productRepository)
        {
            _messageProducer = messageProducer;
            _productRepository = productRepository;
        }

        public Task HandlerAsync(ProductRemovedEvent domainEvent)
        {
            return _messageProducer.SendMessageAsync(domainEvent.ProductId);
        }
    }
}
