using Microsoft.EntityFrameworkCore;
using MShop.Business.Interface;
using MShop.Business.Interface.Service;
using MShop.Business.Service;
using MShop.Business.Validation;
using MShop.Repository.Context;
using MShop.Repository.Repository;
using ApplicationUseCase = MShop.Application.UseCases.Product.DeleteProduct;

namespace MShop.IntegrationTests.Application.UseCase.Product.DeleteProduct
{
    [Collection("Delete Products Collection")]
    [CollectionDefinition("Delete Products Collection", DisableParallelization = true)]
    public class DeleteProductTest : DeleteProductTestFixture, IDisposable
    {
        private readonly RepositoryDbContext _DbContext;
        private readonly ProductRepository _repository;
        private readonly ImagesRepository _imagesRepository;
        private readonly ProductPersistence _productPersistence;
        private readonly IStorageService _storageService;
        private readonly INotification _notification;

        public DeleteProductTest()
        {
            _DbContext = CreateDBContext();
            _repository = new ProductRepository(_DbContext);
            _imagesRepository = new ImagesRepository(_DbContext);
            _productPersistence = new ProductPersistence(_DbContext);
            _storageService = new StorageService();
            _notification = new Notifications();
        }

        [Fact(DisplayName = nameof(DeleteProduct))]
        [Trait("Integration-Application", "Product Use Case")]

        public async Task DeleteProduct()
        {
          
            //var notification = new Notifications();

            var product = Faker();

            //await _DbContext.AddAsync(product);
            //await _DbContext.SaveChangesAsync();
            await _productPersistence.Create(product);

            var useCase = new ApplicationUseCase.DeleteProduct(_repository, _imagesRepository, _notification, _storageService);
            await useCase.Handler(product.Id);

            //var productDbDelete = await CreateDBContext(true).Products.FindAsync(product.Id);
            var productDbDelete = await _productPersistence.GetProduct(product.Id);

            Assert.Null(productDbDelete);

        }

        public void Dispose()
        {
            CleanInMemoryDatabase();
        }
    }
}
