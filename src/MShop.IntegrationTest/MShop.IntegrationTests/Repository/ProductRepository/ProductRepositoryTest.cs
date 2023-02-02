using MShop.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraRepository = MShop.Repository.Repository;

namespace MShop.IntegrationTests.Repository.ProductRepository
{
    public class ProductRepositoryTest: ProductRespositoryTesteFixture
    {
        [Fact(DisplayName = nameof(Insert))]
        [Trait("Intagration - Infra.Data", "Product Repositorio")]

        public async void Insert()
        {
            
            RepositoryDbContext dbContext = CreateDBContext();

            var repository = new InfraRepository.ProductRepository(dbContext);

            var product = Faker();

            await repository.Create(product);

            var newProduct = await dbContext.Products.FindAsync(product.Id);

            Assert.NotNull(newProduct);
            Assert.Equal(product.Id, newProduct.Id);
            Assert.Equal(product.Name, newProduct.Name);
            Assert.Equal(product.Imagem, newProduct.Imagem);
            Assert.Equal(product.Price, newProduct.Price);
            Assert.Equal(product.Stock, newProduct.Stock);
            Assert.Equal(product.CategoryId, newProduct.CategoryId);




        }
    }
}
