using MShop.IntegrationTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity = MShop.Business.Entity;
using MShop.Application.UseCases.Product.CreateProducts;
using MShop.Application.Common;
using MShop.Business.Entity;

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

        protected static FileInput ImageFake()
        {
            return new FileInput("jpg", new MemoryStream(Encoding.ASCII.GetBytes(fakerStatic.Image.LoremPixelUrl())));
        }

        protected CreateProductInPut Faker()
        {
            return new CreateProductInPut
            {
                Name = faker.Commerce.ProductName(),
                Description = faker.Commerce.ProductDescription(),
                Price = Convert.ToDecimal(faker.Commerce.Price()),
                Thumb = ImageFake(),
                CategoryId = _categoryId,
                Stock = faker.Random.UInt(),
                IsActive = true
            };
        }

        protected Category FakeCategory()
        {
            return new Category(faker.Commerce.Categories(1)[0]);
        }
    }
}
