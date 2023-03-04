using MShop.IntegrationTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity = MShop.Business.Entity;
using MShop.Business.ValueObject;

namespace MShop.IntegrationTests.Application.UseCase.Product.ListProduct
{
    public class ListProductTestFixture : BaseFixture
    {
        private readonly Guid _categoryId;
        private readonly Guid _id;
        public ListProductTestFixture() : base()
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
            return product;
        }

        protected List<BusinessEntity.Product> ListFake(int lengt = 5)
        {
            List<BusinessEntity.Product> listProducts = new List<BusinessEntity.Product>();

            for (int i = 0; i < lengt; i++)
                listProducts.Add(Faker());

            return listProducts;
        }
    }
}
