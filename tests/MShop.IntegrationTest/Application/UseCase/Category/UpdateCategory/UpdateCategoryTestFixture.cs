using MShop.Application.UseCases.Category.CreateCategory;
using MShop.Application.UseCases.Category.UpdateCategory;
using MShop.IntegrationTests.Application.UseCase.Category.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.IntegrationTests.Application.UseCase.Category.UpdateCategory
{
    public class UpdateCategoryTestFixture : CategoryTestFixture
    {
        public UpdateCategoryInPut FakerRequest()
        {
            return new UpdateCategoryInPut
            {
                Name = faker.Commerce.Categories(1)[0],
                IsActive = faker.Commerce.Random.Bool()
            };
        }
    }
}
