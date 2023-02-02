using MShop.IntegrationTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity = MShop.Business.Entity;
using MShop.Application.UseCases.Product.CreateProducts;

namespace MShop.IntegrationTests.Application.UseCase.Product.CreateProduct
{
    public abstract class CreateProductTestFixture : BaseFixture
    {
        private readonly Guid _categoryId;
        private readonly Guid _id;
        public CreateProductTestFixture() : base()
        {
            _categoryId = Guid.NewGuid();
            _id = Guid.NewGuid();
        }

        protected CreateProductInPut Faker()
        {
            return new CreateProductInPut
            {
                Name = faker.Commerce.ProductName(),
                Description = faker.Commerce.ProductDescription(),
                Price = Convert.ToDecimal(faker.Commerce.Price()),
                Imagem = faker.Image.LoremPixelUrl(),
                CategoryId = _categoryId,
                Stock = faker.Random.UInt(),
                IsActive = true
            };
        }
    }
}
