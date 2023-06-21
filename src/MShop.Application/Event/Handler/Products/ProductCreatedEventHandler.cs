using MShop.Business.Events.Products;
using MShop.Business.Interface.Event;
using MShop.Business.Interface.Repository;

namespace MShop.Application.Event.Handler.Products
{
    public class ProductCreatedEventHandler : IDomainEventHandler<ProductCreatedEvent>
    {
        private readonly IMessageProducer _messageProducer;
        private readonly IProductRepository _productRepository;
        public ProductCreatedEventHandler(IMessageProducer messageProducer, IProductRepository productRepository)
        {
            _messageProducer = messageProducer;
            _productRepository = productRepository;
        }


        public async Task HandlerAsync(ProductCreatedEvent domainEvent)
        {
            if (domainEvent.ProductId == Guid.Empty) return; 
            var product = await _productRepository.GetProductWithCategory(domainEvent.ProductId);
            if(product == null) return; 
            domainEvent.SetProduct(product);
            await _messageProducer.SendMessageAsync(domainEvent);
        }


    }
}
