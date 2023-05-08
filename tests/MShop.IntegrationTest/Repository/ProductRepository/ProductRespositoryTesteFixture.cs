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

        protected Product Faker(Category category)
        {
            var produto  = new Product
                                (
                                    faker.Commerce.ProductDescription(),
                                    faker.Commerce.ProductName(),
                                    Convert.ToDecimal(faker.Commerce.Price()),
                                    category.Id,
                                    faker.Random.UInt(),
                                    true
                                );
            produto.UpdateCategory(category);
            return produto;
        }

        protected Product FakerImage()
        {
            var product = new Product
            (
                faker.Commerce.ProductDescription(),
                faker.Commerce.ProductName(),
                Convert.ToDecimal(faker.Commerce.Price()),
                _categoryId,
                faker.Random.UInt(),
                true
            );

            product.UpdateThumb(faker.Image.LoremFlickrUrl());
            return product;
        }

        protected List<Product> FakerList(int length = 5) 
        {
            List<Product> listProduct = new List<Product>();
            
            for(int i = 0;i<length; i++)
                listProduct.Add(Faker());

            return listProduct;
        }

        protected List<Product> FakerList(Category category, int length = 5)
        {
            List<Product> listProduct = new List<Product>();

            for (int i = 0; i < length; i++)
                listProduct.Add(Faker(category));

            return listProduct;
        }

        protected Category FakerCategory()
        {
            return new(faker.Commerce.Categories(1)[0],false);
        }

    }
}
