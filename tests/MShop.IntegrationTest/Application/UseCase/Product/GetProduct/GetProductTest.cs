using MShop.Core.Exception;
using MShop.Core.Message;
using MShop.IntegrationTests.Application.UseCase.Category;
using MShop.Repository.Context;
using MShop.Repository.Repository;
using ApplicationUseCase = MShop.Application.UseCases.Product.GetProduct;

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
        private readonly CategoryPersistence _categoryPersistence;
        private readonly INotification _notification;

        public GetProductTest()
        {
            _DbContext = CreateDBContext();
            _repository = new ProductRepository(_DbContext);
            _imagesRepository = new ImagesRepository(_DbContext);
            _productPersistence = new ProductPersistence(_DbContext);
            _categoryPersistence = new CategoryPersistence(_DbContext);
            _notification = new Notifications();
        }

        [Fact(DisplayName = nameof(GetProduct))]
        [Trait("Integration-Application", "Product Use Case")]
        public async Task GetProduct()
        {

            var category = FakeCategory();
            await _categoryPersistence.Create(category);

            var productFake = Faker(category);
            await _productPersistence.Create(productFake);
           
            var guid = productFake.Id;
            var useCase = new ApplicationUseCase.GetProduct(_repository,  _imagesRepository ,_notification);
            var outPut = await useCase.Handle(new ApplicationUseCase.GetProductInPut(guid), CancellationToken.None);

            var result = outPut.Data;

            Assert.False(_notification.HasErrors());
            Assert.NotNull(result);
            Assert.Equal(result.Name, productFake.Name);
            Assert.Equal(result.Description, productFake.Description);
            Assert.Equal(result.Price, productFake.Price);
            Assert.Equal(result.CategoryId, productFake.CategoryId);
            Assert.Equal(result.Stock, productFake.Stock);
            Assert.Equal(result.IsActive, productFake.IsActive);

        }


        [Fact(DisplayName = nameof(SholdReturnErrorWhenCantGetProduct))]
        [Trait("Integration-Application", "Product Use Case")]
        public async Task SholdReturnErrorWhenCantGetProduct()
        {
            var category = FakeCategory();
            await _categoryPersistence.Create(category);

            var productFake = Faker(category);
            await _DbContext.Products.AddAsync(productFake);
            await _DbContext.SaveChangesAsync();

            var useCase = new ApplicationUseCase.GetProduct(_repository, _imagesRepository, _notification);
            //var outPut = async () => await useCase.Handle(new ApplicationUseCase.GetProductInPut(Guid.NewGuid()), CancellationToken.None);
            //var exception = await Assert.ThrowsAsync<ApplicationValidationException>(outPut);
            //Assert.Equal("Error", exception.Message);

            var outPut = await useCase.Handle(new ApplicationUseCase.GetProductInPut(Guid.NewGuid()), CancellationToken.None);

            Assert.True(_notification.HasErrors());
            Assert.False(outPut.IsSuccess);
        }

        public void Dispose()
        {
            CleanInMemoryDatabase();
        }
    }
}
