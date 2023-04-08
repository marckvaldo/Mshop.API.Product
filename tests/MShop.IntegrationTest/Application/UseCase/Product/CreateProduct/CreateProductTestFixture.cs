using MShop.IntegrationTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MShop.Application.UseCases.Product.CreateProducts;
using MShop.Application.Common;
using BusinessEntity = MShop.Business.Entity;
using MShop.IntegrationTests.Application.UseCase.Product.Common;

namespace MShop.IntegrationTests.Application.UseCase.Product.CreateProduct
{
    public abstract class CreateProductTestFixture : ProductTestFixture
    {
        protected CreateProductInPut Faker()
        {
            return new CreateProductInPut
            {
                Name = faker.Commerce.ProductName(),
                Description = faker.Commerce.ProductDescription(),
                Price = Convert.ToDecimal(faker.Commerce.Price()),
                Thumb = ImageFake64(),
                CategoryId = _categoryId,
                Stock = faker.Random.UInt(),
                IsActive = true
            };
        }

        
    }
}
