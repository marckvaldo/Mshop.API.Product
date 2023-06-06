using MShop.Business.Exceptions;
using MShop.Business.Interface;
using MShop.Business.SeedWork;
using MShop.Business.Validator;

namespace MShop.Business.Entity
{
    public class Category : AggregateRoot
    {

        public string Name { get; private set; }

        public bool IsActive { get; private set; }

        public List<Product> Products { get; private set; }

        public Category(string name, bool isActive = true)
        {
            Name = name;
            IsActive = isActive;

        }

        public void IsValid(INotification _notification)
        {
            var categoryValidador = new CategoryValidador(this, _notification);
            categoryValidador.Validate();
            if (_notification.HasErrors())
            {
                throw new EntityValidationException("Validation errors");
            }

        }

        public void Active()
        {
            IsActive = true;
        }

        public void Deactive()
        {
            IsActive = false;
        }

        public void Update(string name)
        {
            Name = name;
        }
    }
}
