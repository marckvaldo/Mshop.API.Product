using MShop.Repository.Repository;
using MShop.Repository.Context;
using ApplicationUseCase = MShop.Application.UseCases.Product.GetProduct;
using MShop.Business.Validation;
using MShop.Business.Exception;
using MShop.Business.Interface;

namespace MShop.IntegrationTests.Application.UseCase.Product.GetProduct
{

    [Collection("Get Products Collection")]
    [CollectionDefinition("Get Products Collection", DisableParallelization = true)]
    public class GetProductTest:GetProductTestFixture, IDisposable
    {
        private readonly RepositoryDbContext _DbContext;
        private readonly ProductRepository _repository;
        private readonly ImagesRepository _imagesRepository;
        private readonly ProductPersistence _productPersistence;
        private readonly INotification _notification;

        public GetProductTest()
        {
            _DbContext = CreateDBContext();
            _repository = new ProductRepository(_DbContext);
            _imagesRepository = new ImagesRepository(_DbContext);
            _productPersistence = new ProductPersistence(_DbContext);
            _notification = new Notifications();
        }

        [Fact(DisplayName = nameof(GetProduct))]
        [Trait("Integration-Application", "Product Use Case")]
        public async Task GetProduct()
        {
           
            var productFake = Faker();
            await _productPersistence.Create(productFake);
           
            var guid = productFake.Id;
            var useCase = new ApplicationUseCase.GetProduct(_repository,  _imagesRepository ,_notification);
            var outPut = await useCase.Handler(guid);


            Assert.False(_notification.HasErrors());
            Assert.NotNull(outPut);
            Assert.Equal(outPut.Name, productFake.Name);
            Assert.Equal(outPut.Description, productFake.Description);
            Assert.Equal(outPut.Price, productFake.Price);
            Assert.Equal(outPut.CategoryId, productFake.CategoryId);
            Assert.Equal(outPut.Stock, productFake.Stock);
            Assert.Equal(outPut.IsActive, productFake.IsActive);

        }


        [Fact(DisplayName = nameof(SholdReturnErrorWhenCantGetProduct))]
        [Trait("Integration-Application", "Product Use Case")]
        public async Task SholdReturnErrorWhenCantGetProduct()
        {
           
            //var notification = new Notifications();

            var productFake = Faker();
            await _DbContext.Products.AddAsync(productFake);
            await _DbContext.SaveChangesAsync();

            var useCase = new ApplicationUseCase.GetProduct(_repository, _imagesRepository, _notification);
            var outPut = async () => await useCase.Handler(Guid.NewGuid());

            var exception = await Assert.ThrowsAsync<NotFoundException>(outPut);
            Assert.Equal("your search returned null", exception.Message);
            Assert.False(_notification.HasErrors());

        }

        public void Dispose()
        {
            CleanInMemoryDatabase();
        }
    }
}
