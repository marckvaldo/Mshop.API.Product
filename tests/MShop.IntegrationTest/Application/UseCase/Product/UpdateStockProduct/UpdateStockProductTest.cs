using MShop.Repository.Repository;
using MShop.Business.Validation;
using MShop.Repository.Context;
using Microsoft.EntityFrameworkCore;
using ApplicationUseCase = MShop.Application.UseCases.Product.UpdateStockProduct;
using MShop.Business.Interface.Service;
using Moq;
using MShop.Business.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MShop.Application.Event;
using MShop.Repository.UnitOfWork;

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
        private readonly DomainEventPublisher _domainEventPublisher;
        private readonly UnitOfWork _unitOfWork;

        public UpdateStockProductTest()
        {
            _DbContext = CreateDBContext();
            _repository = new ProductRepository(_DbContext);
            _categoryRepository = new CategoryRepository(_DbContext);
            _imageRepository = new ImagesRepository(_DbContext);
            _storageService = new Mock<IStorageService>().Object;
            _productPersistence = new ProductPersistence(_DbContext);
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

        [Fact(DisplayName = nameof(UpdateStockProduct))]
        [Trait("Integration-Application", "Product Use Case")]

        public async Task UpdateStockProduct()
        {
            var product = Faker();
            var request = RequestFake();

            await _productPersistence.Create(product);

            var useCase = new ApplicationUseCase.UpdateStockProducts(_repository, _notification, _unitOfWork);
            var outPut = await useCase.Handler(request, CancellationToken.None);
            var outPutDb = await _productPersistence.GetProduct(request.Id);

            Assert.NotNull(outPut);
            Assert.Equal(request.Stock, outPut.Stock);
            Assert.Equal(request.Stock, outPutDb.Stock);

        }

        public void Dispose()
        {
            CleanInMemoryDatabase();
        }
    }
}
