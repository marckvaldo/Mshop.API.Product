using MShop.Application.UseCases.Category.CreateCategory;
using MShop.IntegrationTests.Application.UseCase.Category.Common;
using MShop.IntegrationTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.IntegrationTests.Application.UseCase.Category.CreateCategory
{
    public class CreateCategoryTestFixture : CategoryTestFixture
    {

        public CreateCategoryInPut Faker()
        {
            return new CreateCategoryInPut
            { 
                Name = faker.Commerce.Categories(1)[0], 
                IsActive = faker.Commerce.Random.Bool() 
            };
        }

    }
}
