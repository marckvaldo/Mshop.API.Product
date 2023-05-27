using MShop.Business.Entity;
using MShop.IntegrationTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.IntegrationTests.Repository.UnitOfOwork
{
    public class UnitOfWorkTestFixture : BaseFixture
    {

        protected Product produtcFaker(Guid _categoryId)
        {
            return new Product
            (
                faker.Commerce.ProductDescription(),
                faker.Commerce.ProductName(),
                Convert.ToDecimal(faker.Commerce.Price()),
                _categoryId,
                faker.Random.UInt(),
                true
            );
        }
    }
}
