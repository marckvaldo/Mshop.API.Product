using MShop.Business.SeedWork;

namespace MShop.Business.Events.Products
{
    public class ProductUpdatedEvent : DomainEvent
    {
        public ProductUpdatedEvent(Guid productId)
        {
            ProductId = productId;

        }

        public Guid ProductId { get; private set; }

    }
}
