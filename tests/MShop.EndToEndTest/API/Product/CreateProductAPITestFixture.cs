using BusinessEntity = MShop.Business.Entity;
using UseCase = MShop.Application.UseCases.Product.CreateProducts;
using MShop.EndToEndTest.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.EndToEndTest.API.Product
{
    public class CreateProductAPITestFixture : BaseFixture
    {
        private readonly Guid _categoryId;
        private readonly Guid _id;

        public ProductPersistence Persistence;

        public CreateProductAPITestFixture() : base()
        {
            _categoryId = Guid.NewGuid();
            _id = Guid.NewGuid();

            Persistence = new ProductPersistence(
                CreateDBContext()
                );
        }

        protected BusinessEntity.Product Faker()
        {
            var product = (new BusinessEntity.Product
            (
                faker.Commerce.ProductName(),
                faker.Commerce.ProductDescription(),
                Convert.ToDecimal(faker.Commerce.Price()),
                faker.Image.LoremPixelUrl(),
                _categoryId,
                faker.Random.UInt(),
                true
            ));
            return product;
        }

        protected UseCase.CreateProductInPut Request()
        {
            return new UseCase.CreateProductInPut
            {
                Name = Faker().Name,
                CategoryId = _categoryId,
                Imagem = Faker().Imagem,
                IsActive = true,
                Description = Faker().Description,
                Price = Faker().Price,
                Stock = Faker().Stock
            };
        }

 
    }
}
