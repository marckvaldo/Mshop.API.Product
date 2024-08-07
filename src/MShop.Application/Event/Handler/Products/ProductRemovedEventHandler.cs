﻿using MShop.Business.Events.Products;
using MShop.Core.Message.DomainEvent;
using MShop.Repository.Interface;

namespace MShop.Application.Event.Handler.Products
{
    public class ProductRemovedEventHandler : IDomainEventHandler<ProductRemovedEvent>
    {
        private readonly IMessageProducer _messageProducer;
        private readonly IProductRepository _productRepository;

        public ProductRemovedEventHandler(IMessageProducer messageProducer, IProductRepository productRepository)
        {
            _messageProducer = messageProducer;
            _productRepository = productRepository;
        }

        public async Task HandlerAsync(ProductRemovedEvent domainEvent)
        {
            if (domainEvent.ProductId == Guid.Empty) return;
            await _messageProducer.SendMessageAsync(domainEvent);
        }
    }
}
