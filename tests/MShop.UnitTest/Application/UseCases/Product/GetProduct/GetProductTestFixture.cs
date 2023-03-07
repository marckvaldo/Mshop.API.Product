using Bogus;
using Mshop.Test.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity = MShop.Business.Entity;
using MShop.Business.ValueObject;

namespace Mshop.Tests.Application.UseCases.Product.GetProduct
{
    public class GetProductTestFixture: BaseFixture
    {
        private readonly Guid _categoryId;
        private readonly Guid _id;
        public GetProductTestFixture() : base()
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
                _categoryId,
                faker.Random.UInt(),
                true
            ));
            product.Id = _id;
            product.UpdateThumb(faker.Image.LoremFlickrUrl());
            return product;
        }
    }
}
