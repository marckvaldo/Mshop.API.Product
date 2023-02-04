using Mshop.Test.Common;
using MShop.Application.UseCases.Product.CreateProducts;
using MShop.Business.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationUseCase = MShop.Application.UseCases.Product.CreateProducts;
using BusinessEntity = MShop.Business.Entity;


namespace Mshop.Tests.Application.UseCases.Product.DeleteProduct
{
    public abstract class DeleteProductTestFixture : BaseFixture
    {
        private readonly Guid _categoryId;
        private readonly Guid _id;
        public DeleteProductTestFixture() : base()
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
    }
}
