using Mshop.Test.Common;
using MShop.UnitTests.Common;
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


        public static IEnumerable<object[]> ListNamesCategoryInvalid()
        {
            yield return new object[] { InvalidData.GetNameCategoryGreaterThan30CharactersInvalid() };
            yield return new object[] { InvalidData.GetNameCategoryLessThan3CharactersInvalid() };
            yield return new object[] { "" };
            yield return new object[] { null };
        }


    }

    public class CategoryFake
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsValid { get; set; }

    }
}
