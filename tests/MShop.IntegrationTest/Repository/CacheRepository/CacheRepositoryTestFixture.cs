using MShop.Business.Entity;
using MShop.IntegrationTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.IntegrationTests.Repository.CacheRepository
{
    public class ProductPromotionsTestFixture : BaseFixture
    {
      
        private readonly Guid _categoryId;
        public ProductPromotionsTestFixture() : base()
        {
            _categoryId = Guid.NewGuid();
        }

        protected Product Faker()
        {
            return new Product
            (
                faker.Commerce.ProductDescription(),
                faker.Commerce.ProductName(),
                Convert.ToDecimal(faker.Commerce.Price()),
                faker.Image.LoremPixelUrl(),
                _categoryId,
                faker.Random.UInt(),
                true
            );
        }

        protected List<Product> FakerList(int length = 5)
        {
            List<Product> listProduct = new List<Product>();

            for (int i = 0; i < length; i++)
                listProduct.Add(Faker());

            return listProduct;
        }
    }
}
