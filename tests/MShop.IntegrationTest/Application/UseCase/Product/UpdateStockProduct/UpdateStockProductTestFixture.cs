using Bogus;
using MShop.IntegrationTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity = MShop.Business.Entity;
using ApplicationUseCase = MShop.Application.UseCases.Product.UpdateStockProduct;

namespace MShop.IntegrationTests.Application.UseCase.Product.UpdateStockProduct
{
    public class UpdateStockProductTestFixture : BaseFixture
    {
        private readonly Guid _categoryId;
        private readonly Guid _id;
        public UpdateStockProductTestFixture() : base()
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

        protected ApplicationUseCase.UpdateStockProductInPut RequestFake()
        {
            var product = (new ApplicationUseCase.UpdateStockProductInPut
            {
                Stock = Faker().Stock,
                Id = _id
            });
            return product;
        }

    }
}
