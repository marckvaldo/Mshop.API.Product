using MShop.Business.Events.Products;
using MShop.Business.Interface.Event;
using MShop.Business.Interface.Repository;

namespace MShop.Application.Event.Handler.Products
{
    public class ProductUpdatedEventHandler : IDomainEventHandler<ProductUpdatedEvent>
    {
        private readonly IMessageProducer _messageProducer;
        private readonly IProductRepository _productRepository;

        public ProductUpdatedEventHandler(IMessageProducer messageProducer, IProductRepository productRepository)
        {
            _messageProducer = messageProducer;
            _productRepository = productRepository;
        }

        public Task HandlerAsync(ProductUpdatedEvent domainEvent)
        {
            var product = _productRepository.GetProductWithCategory(domainEvent.ProductId);
            return _messageProducer.SendMessageAsync(product);
        }
    }
}
