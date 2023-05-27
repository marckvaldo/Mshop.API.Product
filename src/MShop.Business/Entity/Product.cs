using MShop.Business.Events;
using MShop.Business.Events.Products;
using MShop.Business.Exceptions;
using MShop.Business.Interface;
using MShop.Business.SeedWork;
using MShop.Business.Validator;
using MShop.Business.ValueObject;

namespace MShop.Business.Entity
{
    public class Product : AggregateRoot
    {
        public string Description { get; private set; }

        public string Name { get; private set; }

        public decimal Price { get; private set; }

        public decimal Stock { get; private set; }

        public bool IsActive { get; private set; }  
        
        public Guid CategoryId { get; private set; }

        public Category Category { get; private set; }

        public FileImage? Thumb { get; private set; }

        public bool IsPromotion { get; private set; }


        public Product(string description, string name, decimal price, Guid categoryId, decimal stock = 0, bool isActive = false) : base()
        {
            Description = description;
            Name = name;
            Price = price;
            Stock = stock;
            IsActive = isActive;
            CategoryId = categoryId;

            ProductCreatedEvent();
        }

        public void IsValid(INotification notification)
        {
            var productValidador = new ProductValidador(this, notification);
            productValidador.Validate();
            if(notification.HasErrors())
            {
                throw new EntityValidationException("Validation errors");
            }
           
        }

        public void Activate()
        {
            IsActive= true;
            ProductCreatedEvent();
        }

        public void Deactive()
        {
            IsActive= false;
            ProductRemovedEvent();
        }

        public void Update(string description, string name, decimal price, Guid categoryId)
        {
            Name = name;
            Description = description;   
            Price = price;
            CategoryId = categoryId;
            ProductUpdatedEvent();
        }

        public void AddQuantityStock(decimal stock)
        {
            Stock += stock;
        }

        public void RemoveQuantityStock(decimal stock)
        {
            Stock -= stock;
        }

        public void UpdateQuantityStock(decimal stock)
        {
            Stock = stock;
        }

        public void UpdateThumb(string thumb)
        {
            Thumb = new FileImage(thumb);
            ProductUpdatedEvent();
        }

        public void ActivatePromotion()
        {
            IsPromotion = true;
            ProductUpdatedEvent();
        }

        public void DeactivePromotion()
        {
            IsPromotion = false;
            ProductUpdatedEvent();
        }
        
        public void UpdateCategory(Category category)
        {
            Category = category;
            CategoryId = category.Id;
            ProductUpdatedEvent();
        }




        private void ProductUpdatedEvent()
        {
            RegisterEvent(new ProductUpdatedEvent(Id));
        }

        private void ProductRemovedEvent()
        {
            RegisterEvent(new ProductRemovedEvent(Id));
        }

        private void ProductCreatedEvent()
        {
            RegisterEvent(new ProductCreatedEvent(Id));
        }

    }
}
