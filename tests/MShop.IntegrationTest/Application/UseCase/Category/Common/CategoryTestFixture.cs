using MShop.IntegrationTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity = MShop.Business.Entity;

namespace MShop.IntegrationTests.Application.UseCase.Category.Common
{
    public class CategoryTestFixture : BaseFixture
    {

        public BusinessEntity.Category Faker()
        {
            return new BusinessEntity.Category
            (
                faker.Commerce.Categories(1)[0],
                faker.Commerce.Random.Bool()
            );
        }

        public List<BusinessEntity.Category> FakerList(int Quantity)
        {
            List<BusinessEntity.Category> categories = new();
            for (int i = 0; i < Quantity; i++)
                categories.Add(Faker());

            return categories;
        }

    }
}
