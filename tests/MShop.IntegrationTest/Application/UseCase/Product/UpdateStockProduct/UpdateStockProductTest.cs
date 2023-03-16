using MShop.Repository.Repository;
using MShop.Business.Validation;
using MShop.Repository.Context;
using Microsoft.EntityFrameworkCore;
using ApplicationUseCase = MShop.Application.UseCases.Product.UpdateStockProduct;
using MShop.Business.Interface.Service;
using Moq;
using MShop.Business.Interface;

namespace MShop.IntegrationTests.Application.UseCase.Product.UpdateStockProduct
{
    [Collection("Update stock Products Collection")]
    [CollectionDefinition("Update stock Products Collection", DisableParallelization = true)]
    public class UpdateStockProductTest : UpdateStockProductTestFixture, IDisposable
    {

        private readonly RepositoryDbContext _DbContext;
        private readonly ProductRepository _repository;
        private readonly CategoryRepository _categoryRepository;
        private readonly ImagesRepository _imageRepository;
        private readonly IStorageService _storageService;
        private readonly INotification _notification;
        private readonly ProductPersistence _productPersistence;

        public UpdateStockProductTest()
        {
            _DbContext = CreateDBContext();
            _repository = new ProductRepository(_DbContext);
            _categoryRepository = new CategoryRepository(_DbContext);
            _imageRepository = new ImagesRepository(_DbContext);
            _storageService = new Mock<IStorageService>().Object;
            _productPersistence = new ProductPersistence(_DbContext);
            _notification = new Notifications();
        }

        [Fact(DisplayName = nameof(UpdateStockProduct))]
        [Trait("Integration-Infra.Data", "Product Use Case")]

        public async Task UpdateStockProduct()
        {
            //var notification = new Notifications();

            var product = Faker();
            var request = RequestFake();

             await _productPersistence.Create(product);
            //await _DbContext.AddAsync(product);
            //await _DbContext.SaveChangesAsync();

            var useCase = new ApplicationUseCase.UpdateStockProducts(_repository, _notification);
            var outPut = await useCase.Handler(request);

            //var productDb = await CreateDBContext(true).Products.AsNoTracking().Where(x => x.Id == product.Id).FirstAsync();
            var productDb = await _productPersistence.GetProduct(product.Id);

            Assert.NotNull(outPut);
            Assert.Equal(request.Stock, outPut.Stock);

        }

        public void Dispose()
        {
            CleanInMemoryDatabase();
        }
    }
}
