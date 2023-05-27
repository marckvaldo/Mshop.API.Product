using MShop.Business.Events.Products;
using MShop.Business.Interface.Event;
using MShop.Business.Interface.Repository;

namespace MShop.Application.Event.Handler.Products
{
    public class SendToProductCreatedEventHandler : IDomainEventHandler<ProductCreatedEvent>
    {
        private readonly IMessageProducer _messageProducer;
        private readonly IProductRepository _productRepository;
        public SendToProductCreatedEventHandler(IMessageProducer messageProducer, IProductRepository productRepository)
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
