using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MShop.Application.Event;
using MShop.Business.Interface;
using MShop.Business.Interface.Service;
using MShop.Business.Service;
using MShop.Business.Validation;
using MShop.Repository.Context;
using MShop.Repository.Repository;
using MShop.Repository.UnitOfWork;
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
        private readonly UnitOfWork _unitOfWork;
        private readonly DomainEventPublisher _domainEventPublisher;

        public DeleteProductTest()
        {
            _DbContext = CreateDBContext();
            _repository = new ProductRepository(_DbContext);
            _imagesRepository = new ImagesRepository(_DbContext);
            _productPersistence = new ProductPersistence(_DbContext);
            _storageService = new StorageService();
            _notification = new Notifications();

            //aqui estou criar um provedor de serviço em tempo de execução
            //criar uma colleção de serviço
            var serviceCollection = new ServiceCollection();
            //adiciona o servico nativo de log
            serviceCollection.AddLogging();
            //constroe um provedor de serviço
            var serviceProvider = serviceCollection.BuildServiceProvider();

            _domainEventPublisher = new DomainEventPublisher(serviceProvider);
            _unitOfWork = new UnitOfWork(_DbContext, _domainEventPublisher, serviceProvider.GetRequiredService<ILogger<UnitOfWork>>());
        }

        [Fact(DisplayName = nameof(DeleteProduct))]
        [Trait("Integration-Application", "Product Use Case")]

        public async Task DeleteProduct()
        {
            var product = Faker();

            await _productPersistence.Create(product);

            var useCase = new ApplicationUseCase.DeleteProduct(
                _repository, 
                _imagesRepository, 
                _notification, 
                _storageService,
                _unitOfWork);

            await useCase.Handle(new ApplicationUseCase.DeleteProductInPut(product.Id), CancellationToken.None);
            var productDbDelete = await _productPersistence.GetProduct(product.Id);

            Assert.Null(productDbDelete);

        }

        public void Dispose()
        {
            CleanInMemoryDatabase();
        }
    }
}
