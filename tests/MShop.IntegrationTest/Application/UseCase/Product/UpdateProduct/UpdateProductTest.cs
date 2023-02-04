using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MShop.Business.Validation;
using MShop.Repository.Context;
using MShop.Repository.Repository;
using ApplicationUseCase = MShop.Application.UseCases.Product.UpdateProduct;

namespace MShop.IntegrationTests.Application.UseCase.Product.UpdateProduct
{
    public class UpdateProductTest : UpdateProdutTestFixture, IDisposable
    {
        

        [Fact(DisplayName = nameof(UpdateProduct))]
        [Trait("Integration-Infra.Data", "Product Use Case")]

        public async void UpdateProduct()
        {
            RepositoryDbContext DbContext = CreateDBContext();

            var repository = new ProductRepository(DbContext);
            var notificacao = new Notifications();

            var product = Faker();
            var request = RequestFake();
            

            DbContext.Add(product);
            await DbContext.SaveChangesAsync();

            var useCase = new ApplicationUseCase.UpdateProduct(repository, notificacao);
            var outPut = await useCase.Handle(request);

            var productDb = await CreateDBContext(true).Products.Where(x=>x.Id == product.Id).FirstAsync();

            Assert.NotNull(outPut);
            Assert.NotNull(productDb);
            Assert.Equal(outPut.Name, productDb.Name);  
            Assert.Equal(outPut.Description, productDb.Description);  
            Assert.Equal(outPut.Imagem, productDb.Imagem);
            Assert.Equal(outPut.Price, productDb.Price);  
            Assert.Equal(outPut.CategoryId, productDb.CategoryId);
            Assert.NotEmpty(outPut.Name);
        }

        public void Dispose()
        {
            CleanInMemoryDatabase();    
        }
    }
}
