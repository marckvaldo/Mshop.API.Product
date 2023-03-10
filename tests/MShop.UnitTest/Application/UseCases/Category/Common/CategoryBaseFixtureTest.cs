using Mshop.Test.Common;
using MShop.Application.UseCases.Category.CreateCategory;
using MShop.UnitTests.Common;
using BusinessEntity = MShop.Business.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.UnitTests.Application.UseCases.Category.common
{
    public class CategoryBaseFixtureTest : BaseFixture
    {
        public CreateCategoryInPut FakerRequest()
        {
            return new CreateCategoryInPut { Name = faker.Commerce.Categories(1)[0], IsActive = true };
        }

        public CreateCategoryInPut FakerRequest(string name, bool isActive)
        {
            return new CreateCategoryInPut { Name = name, IsActive = isActive };
        }

        public BusinessEntity.Category Faker()
        {
            return new BusinessEntity.Category(faker.Commerce.Categories(1)[0]);
        }


        public static IEnumerable<object[]> ListNamesCategoryInvalid()
        {
            yield return new object[] { InvalidData.GetNameCategoryGreaterThan30CharactersInvalid() };
            yield return new object[] { InvalidData.GetNameCategoryLessThan3CharactersInvalid() };
            yield return new object[] { "" };
            yield return new object[] { null };
        }
    }
}
