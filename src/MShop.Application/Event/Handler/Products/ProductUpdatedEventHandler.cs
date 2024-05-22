using MShop.Business.Events.Products;
using MShop.Core.Message.DomainEvent;
using MShop.Repository.Interface;

namespace MShop.Application.Event.Handler.Products
{
    public class ProductUpdatedEventHandler : IDomainEventHandler<ProductUpdatedEvent>
    {
        private readonly IMessageProducer _messageProducer;
        private readonly IProductRepository _productRepository;

        public ProductUpdatedEventHandler(IMessageProducer messageProducer, IProductRepository productRepository)
        {
            _messageProducer = messageProducer;
            //_productRepository = productRepository;
        }

        public async Task HandlerAsync(ProductUpdatedEvent domainEvent)
        {
            if (domainEvent.ProductId == Guid.Empty) return;
            //var product = await _productRepository.GetProductWithCategory(domainEvent.ProductId);
            //if (product is null) return;
            //domainEvent.SetProduct(product);
            await _messageProducer.SendMessageAsync(domainEvent);
        }
    }
}
