
using MShop.Core.DomainObject;

namespace MShop.Business.Events.Products
{
    public class ProductRemovedEvent : DomainEvent
    {
        public ProductRemovedEvent(Guid productId)
        {
            ProductId = productId;
        }

        public Guid ProductId { get; private set; }

    }
}
