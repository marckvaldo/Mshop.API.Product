using BusinessEntity = MShop.Business.Entity;
using UseCase = MShop.Application.UseCases.Product;
using MShop.EndToEndTest.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MShop.Application.UseCases.Product.CreateProducts;

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

        public UseCase.CreateProducts.CreateProductInPut RequestCreate()
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

        public UseCase.UpdateProduct.UpdateProductInPut RequestUpdate()
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


        public List<BusinessEntity.Product> GetProducts(int length = 10)
        {
            List<BusinessEntity.Product> products = new List<BusinessEntity.Product>();
            for (int i = 0; i < length; i++)
                products.Add(Faker());

            return products;

        }

        public string GetDescriptionProductGreaterThan1000CharactersInvalid()
        {
            string description = fakerStatic.Commerce.ProductDescription();
            while (description.Length < 1001)
            {
                description += fakerStatic.Commerce.ProductDescription();
            }

            return description;
        }

        public string GetNameProductGreaterThan255CharactersInvalid()
        {
            string description = fakerStatic.Commerce.ProductName();
            while (description.Length < 256)
            {
                description += fakerStatic.Commerce.ProductName();
            }

            return description;
        }

    }
}
