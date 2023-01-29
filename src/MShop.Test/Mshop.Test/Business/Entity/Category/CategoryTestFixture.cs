using Mshop.Test.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity = MShop.Business.Entity;

namespace Mshop.Test.Business.Entity.Category
{
    public abstract class CategoryTestFixture : BaseFixture
    {
        protected CategoryTestFixture():base()
        {

        }

        protected BusinessEntity.Category GetCategoryValid()
        {
            return new (Fake().Name, Fake().IsActive);
        }

        protected BusinessEntity.Category GetCategoryValid(string name , bool isActive = true)
        {

            return new(name, isActive);
        }

        protected CategoryFake Fake()
        {
           return new CategoryFake 
                    {
                        Name = GetNameCategoryValid(),
                        IsActive = true,
                        IsValid = true
                    }; 
        }

        
        private string GetNameCategoryValid()
        {
            string category = faker.Commerce.Categories(1)[0];
            while(category.Length < 3)
            {
                category = faker.Commerce.Categories(1)[0];
            }

            if (category.Length > 30)
                category = category[..30];

            return category;    
        }


        //Name invalid
        public static IEnumerable<object[]> GetNamesCategoryInvalid()
        {
            yield return new object[] { GetNameCategoryGreaterThan30CharactersInvalid() };
            yield return new object[] { GetNameCategoryLessThan3CharactersInvalid() };
            yield return new object[] { "" };
            yield return new object[] { null };
        }

        private static string GetNameCategoryGreaterThan30CharactersInvalid()
        {
            string category = fakerStatic.Commerce.Categories(1)[0];
            while (category.Length < 30)
            {
                category += fakerStatic.Commerce.Categories(1)[0];
            }

            return category;
        }

        private static string GetNameCategoryLessThan3CharactersInvalid()
        {
            
            string category = fakerStatic.Commerce.Categories(1)[0];
            category = category[..2];
            return category;
        }

    }

    public class CategoryFake
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsValid { get; set; }

    }
}
