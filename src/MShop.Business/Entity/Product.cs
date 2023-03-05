using MShop.Business.Exceptions;
using MShop.Business.Interface;
using MShop.Business.Validation;
using MShop.Business.Validator;
using MShop.Business.ValueObject;
using System.Net;

namespace MShop.Business.Entity
{
    public class Product : Entity
    {
        public string Description { get; private set; }

        public string Name { get; private set; }

        public decimal Price { get; private set; }

        public Image Imagem { get; private set; }

        public decimal Stock { get; private set; }

        public bool IsActive { get; private set; }  
        
        public Guid CategoryId { get; private set; }

        public Category Category { get; private set; }

        public Product(string description, string name, decimal price, Guid categoryId, decimal stock = 0, bool isActive = false)
        {
            Description = description;
            Name = name;
            Price = price;
            Stock = stock;
            IsActive = isActive;
            CategoryId = categoryId;
        }

        public void IsValid(INotification _notification)
        {
            var productValidador = new ProductValidador(this, _notification);
            productValidador.Validate();
            if(_notification.HasErrors())
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

        public void UpdateImage(string image)
        {
            Imagem= new Image(image);
        }
        
    }
}
