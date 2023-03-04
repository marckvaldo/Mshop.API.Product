using Mshop.Test.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity = MShop.Business.Entity;
using ApplicationUseCase = MShop.Application.UseCases.Product.UpdateStockProduct;
using MShop.Application.UseCases.Product.Common;
using MShop.Business.ValueObject;

namespace Mshop.Tests.Application.UseCases.Product.UpdateStockProduct
{
    public class UpdateStockProductTestFixture : BaseFixture
    {
        private readonly Guid _categoryId;
        private readonly Guid _id;

        public UpdateStockProductTestFixture() 
        {
            _categoryId = Guid.NewGuid();
            _id = Guid.NewGuid();
        }

        protected BusinessEntity.Product Faker()
        {
            return (new BusinessEntity.Product
            (
                faker.Commerce.ProductName(),
                faker.Commerce.ProductDescription(),
                Convert.ToDecimal(faker.Commerce.Price()),
                _categoryId,
                faker.Random.UInt(),
                true
            ));
        }

        protected ApplicationUseCase.UpdateStockProductInPut UpdateStockProductInPut()
        {
            return new ApplicationUseCase.UpdateStockProductInPut
            {
                Id = _id,
                Stock = Faker().Stock
            };

        }
    }
}
