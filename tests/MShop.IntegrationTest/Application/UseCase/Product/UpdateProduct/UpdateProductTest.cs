using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MShop.Application.Event;
using MShop.Business.Service;
using MShop.Core.Message;
using MShop.Repository.Context;
using MShop.Repository.Repository;
using MShop.Repository.UnitOfWork;
using ApplicationUseCase = MShop.Application.UseCases.Product.UpdateProduct;

namespace MShop.IntegrationTests.Application.UseCase.Product.UpdateProduct
{
    [Collection("Update Products Collection")]
    [CollectionDefinition("Update Products Collection", DisableParallelization = true)]
    public class UpdateProductTest : UpdateProdutTestFixture, IDisposable
    {

        private readonly RepositoryDbContext _DbContext;
        private readonly RepositoryDbContext _DbContextCategory;
        private readonly ProductRepository _repository;
        private readonly CategoryRepository _categoryRepository;
        private readonly ImagesRepository _imageRepository;
        private readonly StorageService _storageService;
        private readonly UnitOfWork _unitOfWork;
        private readonly DomainEventPublisher _domainEventPublisher;
        private readonly ProductPersistence _productPersistence;

        public UpdateProductTest()
        {
            _DbContext = CreateDBContext();
            _DbContextCategory = CreateDBContext();
            _repository = new ProductRepository(_DbContext);
            _categoryRepository = new CategoryRepository(_DbContext);
            _imageRepository = new ImagesRepository(_DbContext);
            _storageService = new StorageService();
            _productPersistence = new ProductPersistence(_DbContext);


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

        [Fact(DisplayName = nameof(UpdateProduct))]
        [Trait("Integration-Application", "Product Use Case")]

        public async Task UpdateProduct()
        {
           
            var notificacao = new Notifications();

            var category = FakeCategory(Guid.NewGuid());
            var product = Faker(category);
            
            var request = RequestFake(product.Id,category);

            await _productPersistence.Create(product);

            var useCase = new ApplicationUseCase.UpdateProduct(
                _repository, 
                _categoryRepository, 
                notificacao,
                _storageService,
                _unitOfWork);

            var outPut = await useCase.Handle(request, CancellationToken.None);

            var productDb = await CreateDBContext(true).Products.Where(x=>x.Id == product.Id).FirstAsync();

            Assert.NotNull(outPut);
            Assert.NotNull(productDb);
            Assert.Equal(outPut.Name, productDb.Name);  
            Assert.Equal(outPut.Description, productDb.Description);  
            Assert.Equal(outPut.Price, productDb.Price);  
            Assert.Equal(outPut.CategoryId, productDb.CategoryId);
            Assert.NotEmpty(outPut.Name);
        }

        public void Dispose()
        {
            CleanInMemoryDatabase();
        }
    }
}
