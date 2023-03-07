using MShop.Business.Exceptions;
using MShop.Business.Interface;
using MShop.Business.Validation;
using MShop.Business.Validator;
using ValueObject = MShop.Business.ValueObject;
using System.Collections.ObjectModel;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace MShop.Business.Entity
{
    public class Product : Entity
    {
        public string Description { get; private set; }

        public string Name { get; private set; }

        public decimal Price { get; private set; }

        public decimal Stock { get; private set; }

        public bool IsActive { get; private set; }  
        
        public Guid CategoryId { get; private set; }

        public Category Category { get; private set; }

        public ValueObject.Image Thumb { get; private set; }
       

        public Product(string description, string name, decimal price, Guid categoryId, decimal stock = 0, bool isActive = false)
        {
            Description = description;
            Name = name;
            Price = price;
            Stock = stock;
            IsActive = isActive;
            CategoryId = categoryId;
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
        }

        public void Deactive()
        {
            IsActive= false; 
        }

        public void Update(string description, string name, decimal price, Guid categoryId)
        {
            Name = name;
            Description = description;   
            Price = price;
            CategoryId = categoryId;
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
            Thumb = new ValueObject.Image(thumb);
        }
        
    }
}
