using MShop.Business.Validation;
using MShop.Repository.Context;
using MShop.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationUseCase = MShop.Application.UseCases.Product.DeleteProduct;

namespace MShop.IntegrationTests.Application.UseCase.Product.DeleteProduct
{
    public class DeleteProductTest : DeleteProductTestFixture, IDisposable
    {
        private readonly RepositoryDbContext _DbContext;

        public DeleteProductTest()
        {
            _DbContext = CreateDBContext();
        }

        [Fact(DisplayName = nameof(DeleteProduct))]
        [Trait("Integration-Infra.Data", "Product Use Case")]

        public async Task DeleteProduct()
        {
            //RepositoryDbContext dbContext = CreateDBContext();

            var repository = new ProductRepository(_DbContext);
            var notification = new Notifications();

            var product = Faker();
            _DbContext.Add(product);
            await _DbContext.SaveChangesAsync();

            var useCase = new ApplicationUseCase.DeleteProduct(repository,notification);
            await useCase.Handle(product.Id);

            var productDbDelete = await CreateDBContext(true).Products.FindAsync(product.Id);

            Assert.Null(productDbDelete);

        }

        public void Dispose()
        {
            CleanInMemoryDatabase();
        }
    }
}
