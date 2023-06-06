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


        public Task HandlerAsync(ProductCreatedEvent domainEvent)
        {
            var product = _productRepository.GetProductWithCategory(domainEvent.ProductId);
            return _messageProducer.SendMessageAsync(product);
        }


    }
}
