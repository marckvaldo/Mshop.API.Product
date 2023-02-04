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
        [Fact(DisplayName = nameof(DeleteProduct))]
        [Trait("Integration-Infra.Data", "Product Use Case")]

        public async void DeleteProduct()
        {
            RepositoryDbContext dbContext = CreateDBContext();

            var repository = new ProductRepository(dbContext);
            var notification = new Notifications();

            var product = Faker();
            dbContext.Add(product);
            await dbContext.SaveChangesAsync();

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
