using BusinessEntity = MShop.Business.Entity;
using UseCase = MShop.Application.UseCases.Product;
using MShop.EndToEndTest.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.EndToEndTest.API.Product
{
    public class ProductAPITestFixture : BaseFixture
    {
        private readonly Guid _categoryId;
        private readonly Guid _id;

        public ProductPersistence Persistence;

        public ProductAPITestFixture() : base()
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
                faker.Commerce.ProductDescription(),
                faker.Commerce.ProductName(),
                Convert.ToDecimal(faker.Commerce.Price()),
                faker.Image.LoremPixelUrl(),
                _categoryId,
                faker.Random.UInt(),
                true
            ));
            return product;
        }

        protected UseCase.CreateProducts.CreateProductInPut RequestCreate()
        {
            return new UseCase.CreateProducts.CreateProductInPut
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


        protected UseCase.UpdateProduct.UpdateProductInPut RequestUpdate()
        {
            return new UseCase.UpdateProduct.UpdateProductInPut
            {
                Name = Faker().Name,
                CategoryId = _categoryId,
                Imagem = Faker().Imagem,
                IsActive = true,
                Description = Faker().Description,
                Price = Faker().Price,
                Id = _id
            };
        }


    }
}
