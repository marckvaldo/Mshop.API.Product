using MShop.IntegrationTests.Common;
using MShop.Business.Entity;
using MShop.Business.ValueObject;

namespace MShop.IntegrationTests.Repository.ProductRepository
{
    public class ProductRespositoryTesteFixture : BaseFixture
    {

        private readonly Guid _categoryId;
        public ProductRespositoryTesteFixture() : base()
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
                _categoryId,
                faker.Random.UInt(),
                true
            );
        }

        protected List<Product> FakerList(int length = 5) 
        {
            List<Product> listProduct = new List<Product>();
            
            for(int i = 0;i<length; i++)
                listProduct.Add(Faker());

            return listProduct;
        }
    }
}
