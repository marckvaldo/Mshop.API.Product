using MShop.Application.UseCases.Category.CreateCategory;
using MShop.UnitTests.Application.UseCases.Category.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.UnitTests.Application.UseCases.Category.CreateCategory
{
    public class CreateCategoryTestFituxre : CategoryBaseFixtureTest
    {
        public CreateCategoryInPut FakerRequest()
        {
            return new CreateCategoryInPut { Name = faker.Commerce.Categories(1)[0], IsActive = true };
        }

        public CreateCategoryInPut FakerRequest(string name, bool isActive)
        {
            return new CreateCategoryInPut { Name = name, IsActive = isActive };
        }
    }
}
