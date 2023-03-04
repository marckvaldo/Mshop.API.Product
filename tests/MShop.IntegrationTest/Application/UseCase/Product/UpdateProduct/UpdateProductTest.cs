using Microsoft.EntityFrameworkCore;
using MShop.Business.Validation;
using MShop.Repository.Context;
using MShop.Repository.Repository;
using ApplicationUseCase = MShop.Application.UseCases.Product.UpdateProduct;

namespace MShop.IntegrationTests.Application.UseCase.Product.UpdateProduct
{
    [Collection("Update Products Collection")]
    [CollectionDefinition("Update Products Collection", DisableParallelization = true)]
    public class UpdateProductTest : UpdateProdutTestFixture, IDisposable
    {

        private readonly RepositoryDbContext _DbContext;
        private readonly ProductRepository _repository;

        public UpdateProductTest()
        {
            _DbContext = CreateDBContext();
            _repository = new ProductRepository(_DbContext);
        }

        [Fact(DisplayName = nameof(UpdateProduct))]
        [Trait("Integration-Infra.Data", "Product Use Case")]

        public async Task UpdateProduct()
        {
           
            var notificacao = new Notifications();

            var product = Faker();
            var request = RequestFake();
            

            await _DbContext.AddAsync(product);
            await _DbContext.SaveChangesAsync();

            var useCase = new ApplicationUseCase.UpdateProduct(_repository, notificacao);
            var outPut = await useCase.Handle(request);

            var productDb = await CreateDBContext(true).Products.Where(x=>x.Id == product.Id).FirstAsync();

            Assert.NotNull(outPut);
            Assert.NotNull(productDb);
            Assert.Equal(outPut.Name, productDb.Name);  
            Assert.Equal(outPut.Description, productDb.Description);  
            Assert.Equal(outPut.Imagem, productDb.Imagem.Path);
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
