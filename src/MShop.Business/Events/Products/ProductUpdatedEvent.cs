using MShop.Business.Entity;
using MShop.Business.ValueObject;
using MShop.Core.DomainObject;

namespace MShop.Business.Events.Products
{
    public class ProductUpdatedEvent : DomainEvent
    {
        public ProductUpdatedEvent(Guid productId, string description, string name, decimal price, decimal stock, bool isActive, Guid categoryId, string category, string thumb, bool isSale)
        {
            ProductId = productId;
            Description = description;
            Name = name;
            Price = price;
            Stock = stock;
            IsActive = isActive;
            CategoryId = categoryId;
            Category = category;
            Thumb = thumb;
            IsSale = isSale;
        }

        public Guid ProductId { get; private set; }

        public string Description { get; private set; }

        public string Name { get; private set; }

        public decimal Price { get; private set; }

        public decimal Stock { get; private set; }

        public bool IsActive { get; private set; }

        public Guid CategoryId { get; private set; }

        public string Category { get; private set; }

        public string? Thumb { get; private set; }

        public bool IsSale { get; private set; }

    }
}
