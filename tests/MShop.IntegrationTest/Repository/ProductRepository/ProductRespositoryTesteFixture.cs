using Microsoft.EntityFrameworkCore;
using MShop.IntegrationTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MShop.Business.Entity;
using MShop.Repository.Context;

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
                faker.Image.LoremPixelUrl(),
                _categoryId,
                faker.Random.UInt(),
                true
            );
        }

        /*
        protected RepositoryDbContext CreateDBContext(bool preserveData = false)
        {

            var context =  new RepositoryDbContext(
                new DbContextOptionsBuilder<RepositoryDbContext>()
                .UseInMemoryDatabase("integration-test-db")
                .Options
                );

            if (!preserveData)
                context.Database.EnsureDeleted();

            return context;

        }

        protected void CleanInMemoryDatabase()
        {
            CreateDBContext().Database.EnsureDeleted();
        }
        */


        protected List<Product> FakerList(int length = 5) 
        {
            List<Product> listProduct = new List<Product>();
            
            for(int i = 0;i<length; i++)
                listProduct.Add(Faker());

            return listProduct;
        }


        

    }
}
