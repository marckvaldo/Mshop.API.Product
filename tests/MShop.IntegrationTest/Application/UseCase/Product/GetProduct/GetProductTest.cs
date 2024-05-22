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
            var category = FakeCategory();
            await _categoryPersistence.Create(category);

            var productFake = Faker(category);
            await _DbContext.Products.AddAsync(productFake);
            await _DbContext.SaveChangesAsync();

            var useCase = new ApplicationUseCase.GetProduct(_repository, _imagesRepository, _notification);
            var outPut = async () => await useCase.Handle(new ApplicationUseCase.GetProductInPut(Guid.NewGuid()), CancellationToken.None);

            var exception = await Assert.ThrowsAsync<ApplicationValidationException>(outPut);
            Assert.Equal("Error", exception.Message);
            Assert.True(_notification.HasErrors());

        }

        public void Dispose()
        {
            CleanInMemoryDatabase();
        }
    }
}
