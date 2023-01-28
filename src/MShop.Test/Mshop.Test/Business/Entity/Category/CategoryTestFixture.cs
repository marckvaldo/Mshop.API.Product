using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity = MShop.Business.Entity;

namespace Mshop.Test.Business.Entity.Category
{
    public abstract class CategoryTestFixture
    {
        public BusinessEntity.Category GetCategoryValid()
        {
            
            return new (Fake().Name, Fake().isActive);
        }

        public BusinessEntity.Category GetCategoryValid(string name , bool isActive = true)
        {

            return new(name, isActive);
        }

        public CategoryFake Fake()
        {
           return new CategoryFake 
                    {
                        Name = "Category Name",
                        isActive = true,
                        isValid = true
                    };
           
        }

    }

    public class CategoryFake
    {
        public string Name { get; set; }
        public bool isActive { get; set; }
        public bool isValid { get; set; }

    }
}
