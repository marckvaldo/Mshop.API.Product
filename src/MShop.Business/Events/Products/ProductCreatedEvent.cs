using MShop.Business.SeedWork;

namespace MShop.Business.Events.Products
{
    public class ProductCreatedEvent : DomainEvent
    {
        public ProductCreatedEvent(Guid productId)
        {
            ProductId = productId;
        }

        public Guid ProductId { get; private set; }

    }
}
