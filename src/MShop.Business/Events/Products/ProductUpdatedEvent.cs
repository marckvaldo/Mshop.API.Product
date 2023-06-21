using MShop.Business.Entity;
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
        public Product Product { get; private set; }

        public void SetProduct(Product product) => Product = product;

    }
}
