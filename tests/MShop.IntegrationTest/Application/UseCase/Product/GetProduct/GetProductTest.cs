using MShop.Repository.Repository;
using MShop.Repository.Context;
using ApplicationUseCase = MShop.Application.UseCases.Product.GetProduct;
using MShop.Business.Validation;
using MShop.Business.Exception;

namespace MShop.IntegrationTests.Application.UseCase.Product.GetProduct
{

    [Collection("Get Products Collection")]
    [CollectionDefinition("Get Products Collection", DisableParallelization = true)]
    public class GetProductTest:GetProductTestFixture, IDisposable
    {
        private readonly RepositoryDbContext _DbContext;
        private readonly ProductRepository _repository;

        public GetProductTest()
        {
            _DbContext = CreateDBContext();
            _repository = new ProductRepository(_DbContext);
        }

        [Fact(DisplayName = nameof(GetProduct))]
        [Trait("Integration-Infra.Data", "Product Use Case")]
        public async Task GetProduct()
        {
            var notification = new Notifications();

            var productFake = Faker();
            await _DbContext.Products.AddAsync(productFake);
            await _DbContext.SaveChangesAsync();

            var guid = productFake.Id;
            var useCase = new ApplicationUseCase.GetProduct(_repository, notification);
            var outPut = await useCase.Handle(guid);


            Assert.False(notification.HasErrors());
            Assert.NotNull(outPut);
            Assert.Equal(outPut.Name, productFake.Name);
            Assert.Equal(outPut.Description, productFake.Description);
            Assert.Equal(outPut.Price, productFake.Price);
            Assert.Equal(outPut.Imagem, productFake.Imagem);
            Assert.Equal(outPut.CategoryId, productFake.CategoryId);
            Assert.Equal(outPut.Stock, productFake.Stock);
            Assert.Equal(outPut.IsActive, productFake.IsActive);

        }


        [Fact(DisplayName = nameof(SholdReturnErrorWhenCantGetProduct))]
        [Trait("Integration-Infra.Data", "Product Use Case")]
        public async Task SholdReturnErrorWhenCantGetProduct()
        {
           
            var notification = new Notifications();

            var productFake = Faker();
            await _DbContext.Products.AddAsync(productFake);
            await _DbContext.SaveChangesAsync();

            var useCase = new ApplicationUseCase.GetProduct(_repository, notification);
            var outPut = async () => await useCase.Handle(Guid.NewGuid());

            var exception = await Assert.ThrowsAsync<NotFoundException>(outPut);
            Assert.Equal("your search returned null", exception.Message);
            Assert.False(notification.HasErrors());

        }

        public void Dispose()
        {
            CleanInMemoryDatabase();
        }
    }
}
