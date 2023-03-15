using MShop.Business.Entity;
using MShop.IntegrationTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.IntegrationTests.Repository.CategoryRepository
{
    public class CategoryRepositoryTestFixture : BaseFixture
    {
        public CategoryRepositoryTestFixture() { }  

        public Category Faker()
        {
            return new Category(faker.Commerce.Categories(1)[0], true);
        }

        public IEnumerable<Category> FakerCategories(int quantity) 
        { 
            List<Category> categories = new ();
            for (int i = 0; i < quantity; i++)
                categories.Add(Faker());

            return categories;
                
        }
    }
}
