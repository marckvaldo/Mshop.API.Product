
using MShop.Core.DomainObject;

namespace MShop.Business.Events.Products
{
    public class ProductRemovedEvent : DomainEvent
    {
        public ProductRemovedEvent(Guid productId, Guid categoryId)
        {
            ProductId = productId;
            CategoryId = categoryId;    
        }

        public Guid ProductId { get; private set; }
        public Guid CategoryId { get; private set; }    

    }
}
