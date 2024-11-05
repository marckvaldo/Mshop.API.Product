using MShop.Business.Events.Category;
using MShop.Business.Validator;
using MShop.Core.Exception;
using Core = MShop.Core.Message;


namespace MShop.Business.Entity
{
    public class Category : Core.DomainObject.Entity
    {

        public string Name { get; private set; }

        public bool IsActive { get; private set; }

        //Entity
        public List<Product> Products { get; private set; }

        public Category(string name, bool isActive = true)
        {
            Name = name;
            IsActive = isActive;

        }

        /*public override void IsValid(Core.Message.INotification notification)
        {
            var categoryValidador = new CategoryValidador(this, notification);
            categoryValidador.Validate();
            if (notification.HasErrors())
            {
                throw new EntityValidationException("Validation errors");
            }
        }*/

        public override bool IsValid(Core.Message.INotification notification)
        {
            var categoryValidador = new CategoryValidador(this, notification);
            categoryValidador.Validate();
            return !notification.HasErrors();
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

        public void CategoryUpdateEvent(CategoryUpdateEvent categoryEvent) 
        {
            RegisterEvent(categoryEvent);
        }
    }
}
