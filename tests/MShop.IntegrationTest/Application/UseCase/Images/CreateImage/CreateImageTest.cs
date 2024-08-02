using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MShop.Application.Event;
using MShop.Business.Interface.Service;
using MShop.Business.Service;
using MShop.Core.Message;
using MShop.Core.Message.DomainEvent;
using MShop.IntegrationTests.Application.UseCase.Category;
using MShop.IntegrationTests.Application.UseCase.Images.Commom;
using MShop.IntegrationTests.Application.UseCase.Images.CreateImage;
using MShop.IntegrationTests.Application.UseCase.Product;
using MShop.Repository.Context;
using MShop.Repository.Interface;
using MShop.Repository.Repository;
using MShop.Repository.UnitOfWork;
using ApplicationUseCase = MShop.Application.UseCases.Images.CreateImage;
using MShop.Core.Data;

namespace MShop.IntegrationTests.Application.UseCase.Images.CreateImages
{
    [Collection("Create Image Collection")]
    [CollectionDefinition("Create Image Collection", DisableParallelization = true)]
    public class CreateImageTest : CreateImageTestFixture, IDisposable
    {
        private readonly IImageRepository _imageRepository;
        private readonly IProductRepository _productRepository;
        private readonly INotification _notification;
        private readonly RepositoryDbContext _repositoryContext;
        private readonly IStorageService _storageService;
        private readonly ImagePersistense _imagePersistense;
        private readonly CategoryPersistence _categoryPersistence;
        private readonly ProductPersistence _productPersistence;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDomainEventPublisher _domainEventPublisher;

        public CreateImageTest()
        {
            _repositoryContext = CreateDBContext();
            _imageRepository = new ImagesRepository(_repositoryContext);
            _productRepository = new ProductRepository(_repositoryContext);
            _notification = new Notifications();
            _storageService = new StorageService();
            _imagePersistense = new ImagePersistense(_repositoryContext);
            _categoryPersistence = new CategoryPersistence(_repositoryContext); 
            _productPersistence = new ProductPersistence(_repositoryContext);

            //aqui estou criar um provedor de serviço em tempo de execução
            //criar uma colleção de serviço
            var serviceCollection = new ServiceCollection();
            //adiciona o servico nativo de log
            serviceCollection.AddLogging();
            //constroe um provedor de serviço
            var serviceProvider = serviceCollection.BuildServiceProvider();

            _domainEventPublisher = new DomainEventPublisher(serviceProvider);
            _unitOfWork = new UnitOfWork(_repositoryContext, _domainEventPublisher, serviceProvider.GetRequiredService<ILogger<UnitOfWork>>());
        }

        [Fact(DisplayName = nameof(CreateImage))]
        [Trait("Integration-Application", "Image Use Case")]
        public async void CreateImage()
        {
            var category = FakerCategory();
            await _categoryPersistence.Create(category);

            var product = FakerProduct(category);
            await _productPersistence.Create(product);  

            var request = FakerRequest(product.Id);
            var useCase = new ApplicationUseCase.CreateImage(
                _imageRepository, 
                _storageService, 
                _productRepository, 
                _notification,
                _unitOfWork);

            var outPut = await useCase.Handle(request,CancellationToken.None);

            var result = outPut.Data;

            Assert.NotNull(result);
            Assert.True(result.Images.Count == 3);
        }

        public void Dispose()
        {
            CleanInMemoryDatabase();
        }
    }
}
