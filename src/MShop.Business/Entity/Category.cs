using MShop.Business.Exceptions;
using MShop.Business.Interface;
using MShop.Business.Validation;
using MShop.Business.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Business.Entity
{
    public class Category : Entity
    {

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public List<Product> products { get; set; } 

        public Category(string name, bool isActive)
        {
            Name = name;
            IsActive = isActive;
           
        }
       
        public void IsValid(INotification _notification)
        {
            var categoryValidador = new CategoryValidador(this,_notification);
            categoryValidador.Validate();
            if(_notification.HasErrors())
            {
                throw new EntityValidationException("Validation errors");
            }

        }

        public void Active()
        {
            IsActive= true;
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
