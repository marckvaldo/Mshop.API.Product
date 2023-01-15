using MShop.Business.Validation;
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

        public Category(string name, bool isActive)
        {
            Name = name;
            IsActive = isActive;
            IsValidade();
        }
       
        public void IsValidade()
        {
            //ValidationDefault.NotNullOrEmpty(Name, nameof(Name));
            //ValidationDefault.MaxLength(Name, 30, nameof(Name));
            //ValidationDefault.MinLength(Name, 3, nameof(Name));
        }

        public void Active()
        {
            IsActive= true;
            IsValidade();
        }

        public void Deactive()
        {
            IsActive = false;
            IsValidade();
        }

        public void Update(string name)
        {
            Name = name;
            IsValidade();
        }
    }
}
