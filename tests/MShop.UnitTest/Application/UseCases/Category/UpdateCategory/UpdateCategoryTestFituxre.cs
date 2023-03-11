using MShop.Application.UseCases.Category.CreateCategory;
using MShop.Application.UseCases.Category.UpdateCategory;
using MShop.UnitTests.Application.UseCases.Category.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.UnitTests.Application.UseCases.Category.UpdateCategory
{
    public class UpdateCategoryTestFituxre : CategoryBaseFixtureTest
    {
        public UpdateCategoryInPut FakerRequest()
        {
            return new UpdateCategoryInPut { Name = faker.Commerce.Categories(1)[0], IsActive = true };
        }

        public UpdateCategoryInPut FakerRequest(string name, bool isActive)
        {
            return new UpdateCategoryInPut { Name = name, IsActive = isActive };
        }
    }
}
