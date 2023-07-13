using MShop.Business.Entity;
using MShop.Business.SeedWork;
using MShop.Business.ValueObject;

namespace MShop.Business.Events.Products
{
    public class ProductCreatedEvent : DomainEvent
    {
        public ProductCreatedEvent(Guid productId)
        {
            ProductId = productId;
        }

        public Guid ProductId { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public decimal Stock { get; set; }

        public bool IsActive { get; set; }

        public Guid CategoryId { get; set; }

        public Category Category { get; set; }

        public FileImage? Thumb { get; set; }

        public bool IsSale { get; set; }

        public void SetProduct(Product product)
        {
            Description = product.Description;
            Name = product.Name;    
            Price = product.Price;
            Stock = product.Stock;
            IsActive = product.IsActive;
            CategoryId = product.CategoryId;
            Thumb = product.Thumb;
            IsActive = product.IsActive;
            IsSale = product.IsSale;
            Category = product.Category;
        }

    }
}
