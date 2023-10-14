using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MShop.Application.Event;
using MShop.Business.Exceptions;
using MShop.Business.Interface;
using MShop.Business.Service;
using MShop.Business.Validation;
using MShop.IntegrationTests.Application.UseCase.Category;
using MShop.Repository.Context;
using MShop.Repository.Repository;
using MShop.Repository.UnitOfWork;
using ApplicationUseCase = MShop.Application.UseCases.Product.CreateProducts;

namespace MShop.IntegrationTests.Application.UseCase.Product.CreateProduct
{
    [Collection("Create Products Collection")]
    [CollectionDefinition("Create Products Collection", DisableParallelization = true)]
    public class CreateProductTest : CreateProductTestFixture, IDisposable
    {

        private readonly RepositoryDbContext _DbContext;
        private readonly ProductRepository _repository;
        private readonly CategoryRepository _categoryRepository;
        private readonly ImagesRepository _imageRepository;
        private readonly StorageService _storageService;
        private readonly ProductPersistence _productPersistence;
        private readonly CategoryPersistence _categoryPersistence;
        private readonly INotification _notification;
        private readonly UnitOfWork _unitOfWork;
        private readonly DomainEventPublisher _domainEventPublisher;

        public CreateProductTest()
        {
            _DbContext = CreateDBContext();
            _repository = new ProductRepository(_DbContext);
            _categoryRepository = new CategoryRepository(_DbContext);
            _imageRepository= new ImagesRepository(_DbContext);
            _storageService = new StorageService();
            _productPersistence = new ProductPersistence(_DbContext);
            _categoryPersistence = new CategoryPersistence(_DbContext);
            _notification = new Notifications();

            //dados faker
            //aqui estou criando um provedor de serviço em tempo de execução
            //criar uma colleção de serviço
            var serviceCollection = new ServiceCollection();
            //adiciona o servico nativo de log
            serviceCollection.AddLogging();
            //constroe um provedor de serviço
            var serviceProvider = serviceCollection.BuildServiceProvider();

            _domainEventPublisher = new DomainEventPublisher(serviceProvider);  
            _unitOfWork = new UnitOfWork(_DbContext, _domainEventPublisher, serviceProvider.GetRequiredService<ILogger<UnitOfWork>>());

        }


        [Fact(DisplayName = nameof(CreateProduct))]
        [Trait("Integration-Application", "Product Use Case")]
        public async Task CreateProduct()
        {         
            var request = Faker();
            var categoryFake = FakeCategory();

            await _categoryPersistence.Create(categoryFake);
            var categoryDb = await _categoryPersistence.GetCategory(categoryFake.Id);
            Assert.NotNull(categoryDb);
            request.CategoryId = categoryDb.Id;

            var productUseCase = new ApplicationUseCase.CreateProduct(
                _repository, 
                _notification,
                _categoryRepository, 
                _storageService, 
                _unitOfWork);

            var outPut = await productUseCase.Handle(request, CancellationToken.None);
            var newProduct = await _productPersistence.GetProduct(outPut.Id);
            
            Assert.False(_notification.HasErrors());
            Assert.NotNull(outPut);
            Assert.NotNull(newProduct);
            Assert.Equal(outPut.Name, newProduct.Name);
            Assert.Equal(outPut.Description, newProduct.Description);
            Assert.Equal(outPut.Price, newProduct.Price);
            Assert.Equal(outPut.CategoryId, newProduct.CategoryId);
            Assert.Equal(outPut.Stock, newProduct.Stock);
            Assert.Equal(outPut.IsActive, newProduct.IsActive);

            Assert.Equal(request.Name, outPut.Name);
            Assert.Equal(request.Description, outPut.Description);
            Assert.Equal(request.Price, outPut.Price);
            Assert.Equal(request.CategoryId, outPut.CategoryId);
            Assert.Equal(request.Stock, outPut.Stock);
            Assert.Equal(request.IsActive, outPut.IsActive);

        }


        [Fact(DisplayName = nameof(SholdReturnErrorWhenCreateProductWithOutCategory))]
        [Trait("Integration-Application", "Product Use Case")]
        public async Task SholdReturnErrorWhenCreateProductWithOutCategory()
        {
            var request = Faker();

            request.CategoryId = Guid.NewGuid();

            var productUseCase = new ApplicationUseCase.CreateProduct(
                _repository, 
                _notification, 
                _categoryRepository, 
                _storageService, 
                _unitOfWork);

            var outPut = async () => await productUseCase.Handle(request, CancellationToken.None);

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
