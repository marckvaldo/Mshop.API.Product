using MShop.Business.Interface;
using MShop.Business.Validation;

namespace MShop.Business.Entity
{
    public class Product : Entity
    {
        public string Description { get; private set; }

        public string Name { get; private set; }

        public decimal Price { get; private set; }

        public string? Imagem { get; private set; } 

        public decimal Stock { get; private set; }

        public bool IsActive { get; private set; }  
        
        public Guid CategoryId { get; private set; }

        private readonly INotification _notification;

        public Product(string description, string name, decimal price, string? imagem, Guid categoryId, decimal stock = 0, bool isActive = true)
        {
            Description = description;
            Name = name;
            Price = price;
            Imagem = imagem;
            Stock = stock;
            IsActive = isActive;
            CategoryId = categoryId;
        }

        private void IsValid()
        {
            //Description
            /*ValidationDefault.NotNullOrEmpty(Description, nameof(Description));
            ValidationDefault.MinLength(Description,10,nameof(Description));
            ValidationDefault.MaxLength(Description, 255 ,nameof(Description));

            //Name
            ValidationDefault.NotNullOrEmpty(Name, nameof(Name));
            ValidationDefault.MinLength(Name,3,nameof(Name));   
            ValidationDefault.MaxLength(Name, 30, nameof(Name));

            //Price
            ValidationDefault.IsPositiveNumber(Price, nameof(Price));*/

        }

        public void Activate()
        {
           IsActive= true;
           IsValid();
        }

        public void Deactive()
        {
            IsActive= false; 
            IsValid(); 
        }

        public void Update(string description, string name, decimal price)
        {
            Name = name;
            Description = description;   
            Price = price;  
            IsValid();  
        }

        public void AddQuantityStock(decimal stock)
        {
            Stock += stock;
            IsValid();
        }

        public void RemoveQuantityStock(decimal stock)
        {
            Stock -= stock;
            IsValid();
        }

        public void UpdateQuantityStock(decimal stock)
        {
            Stock = stock;
            IsValid();
        }

        public void updateImage(string image)
        {
            Imagem= image;
            IsValid();
        }
        
    }
}
