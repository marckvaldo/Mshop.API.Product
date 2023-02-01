using Bogus;
using Mshop.Test.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity = MShop.Business.Entity;
using ApplicationUseCase = MShop.Application.UseCases.Product.UpdateProduct;
using MShop.Application.UseCases.Product.Common;


namespace Mshop.Tests.Application.UseCases.Product.UpdateProduct
{
    public class UpdateProductTestFixture : BaseFixture
    {
        private readonly Guid _categoryId;
        private readonly Guid _id;
        public UpdateProductTestFixture() : base()
        {
            _categoryId = Guid.NewGuid();
            _id = Guid.NewGuid();
        }

        protected ApplicationUseCase.UpdateProductInPut ProductInPut()
        {
            return new ApplicationUseCase.UpdateProductInPut
            {
                Id = _id,
                Name = Faker().Name,
                Description = Faker().Description,
                Price = Faker().Price,
                Imagem = Faker().Imagem,
                CategoryId = Faker().CategoryId,
                IsActive = Faker().IsActive
            };
            
        }


        protected ProductModelOutPut ProductModelOutPut()
        {
            return new ProductModelOutPut
            ( 
                _id,
                Faker().Description,
                Faker().Name,
                Faker().Price,
                Faker().Imagem,
                Faker().Stock,
                Faker().IsActive,
                Faker().CategoryId
            );

        }


        protected BusinessEntity.Product Faker()
        {
            return (new BusinessEntity.Product
            (
                faker.Commerce.ProductName(),
                faker.Commerce.ProductDescription(),
                Convert.ToDecimal(faker.Commerce.Price()),
                faker.Image.LoremPixelUrl(),
                _categoryId,
                faker.Random.UInt(),
                true
            ));
        }
    }
}
