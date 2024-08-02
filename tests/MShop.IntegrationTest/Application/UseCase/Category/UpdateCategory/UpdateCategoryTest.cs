using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MShop.Application.Event;
using MShop.Core.Message;
using MShop.Repository.Context;
using MShop.Repository.Interface;
using MShop.Repository.Repository;
using MShop.Repository.UnitOfWork;
using ApplicationUseCase = MShop.Application.UseCases.Category.UpdateCategory;

namespace MShop.IntegrationTests.Application.UseCase.Category.UpdateCategory
{
    [Collection("Update Category Collection")]
    [CollectionDefinition("Update Category Collection", DisableParallelization = true)]
    public class UpdateCategoryTest : UpdateCategoryTestFixture, IDisposable
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly INotification _notification;
        private readonly RepositoryDbContext _context;
        private readonly CategoryPersistence _categoryPersistence;
        private readonly UnitOfWork _unitOfWork;
        private readonly DomainEventPublisher _domainEventPublisher;

        public UpdateCategoryTest()
        {
            _context = CreateDBContext();
            _categoryRepository = new CategoryRepository(_context);
            _productRepository = new ProductRepository(_context);
            _notification = new Notifications();
            _categoryPersistence = new CategoryPersistence(_context);

            //aqui estou criar um provedor de serviço em tempo de execução
            //criar uma colleção de serviço
            var serviceCollection = new ServiceCollection();
            //adiciona o servico nativo de log
            serviceCollection.AddLogging();
            //constroe um provedor de serviço
            var serviceProvider = serviceCollection.BuildServiceProvider();

            _domainEventPublisher = new DomainEventPublisher(serviceProvider);
            _unitOfWork = new UnitOfWork(_context, _domainEventPublisher, serviceProvider.GetRequiredService<ILogger<UnitOfWork>>());
        }

        [Fact(DisplayName = nameof(UpdateCategory))]
        [Trait("Integration-Application", "Category Use Case")]

        public async Task UpdateCategory()
        {

            var category = Faker();
            var request = FakerRequest();
            request.Id = category.Id;

             await _categoryPersistence.Create(category);

            var useCase = new ApplicationUseCase.UpdateCategory(
                _categoryRepository, 
                _notification,
                _unitOfWork);
            var outPut = await useCase.Handle(request, CancellationToken.None);

            var categoryDb = await _categoryPersistence.GetCategory(category.Id);

            var result = outPut.Data;

            Assert.NotNull(result);
            Assert.NotNull(categoryDb);
            Assert.Equal(result.Name, categoryDb.Name);
            Assert.NotEmpty(result.Name);
        }

        public void Dispose()
        {
            CleanInMemoryDatabase();
        }
    }
}
