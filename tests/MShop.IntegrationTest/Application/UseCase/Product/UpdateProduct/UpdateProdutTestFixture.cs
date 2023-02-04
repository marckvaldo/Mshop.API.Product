using MShop.IntegrationTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity = MShop.Business.Entity;
using ApplicationUseCase = MShop.Application.UseCases.Product.UpdateProduct;

namespace MShop.IntegrationTests.Application.UseCase.Product.UpdateProduct
{
    public class UpdateProdutTestFixture : BaseFixture
    {
        private readonly Guid _categoryId;
        private readonly Guid _id;
        public UpdateProdutTestFixture() : base()
        {
            _categoryId = Guid.NewGuid();
            _id = Guid.NewGuid();
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
            product.Id = _id;
            return product;
        }


        protected ApplicationUseCase.UpdateProductInPut RequestFake()
        {
            var product = (new ApplicationUseCase.UpdateProductInPut
            {
                Description = Faker().Description,
                Name = Faker().Name,
                Price = Convert.ToDecimal(Faker().Price),
                Imagem = Faker().Imagem,
                CategoryId = _categoryId,
                IsActive = true,
                Id = _id    
            });
            return product;
        }
    }
}
