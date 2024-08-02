using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MShop.Application.Event;
using MShop.Core.Exception;
using MShop.Core.Message;
using MShop.Repository.Context;
using MShop.Repository.Interface;
using MShop.Repository.Repository;
using MShop.Repository.UnitOfWork;
using ApplicationUseCase = MShop.Application.UseCases.Category.CreateCategory;

namespace MShop.IntegrationTests.Application.UseCase.Category.CreateCategory
{

    [Collection("Create Category Collection")]
    [CollectionDefinition("Create Category Collection", DisableParallelization = true)]
    public class CreateCategoryTest:CreateCategoryTestFixture, IDisposable
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly INotification _notification;
        private readonly RepositoryDbContext _context;
        private readonly CategoryPersistence _categoryPersistence;
        private readonly UnitOfWork _unitOfWork;
        private readonly DomainEventPublisher _domainEventPublisher;

        public CreateCategoryTest()
        {
            _context = CreateDBContext();
            _categoryRepository = new CategoryRepository(_context);
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

        [Fact(DisplayName = nameof(CreateCategory))]
        [Trait("Integration-Application", "Category Use Case")]
        public async void CreateCategory()
        {
            var request = Faker();
            var useCase = new ApplicationUseCase.CreateCategory(
                _notification,
                _categoryRepository,
                _unitOfWork);

            var outPut = await useCase.Handle(request, CancellationToken.None);

            var categoryDB = await _categoryPersistence.GetCategory(outPut.Data.Id);

            Assert.NotNull(outPut?.Data);
            Assert.Equal(outPut?.Data?.Name, categoryDB.Name);
            Assert.Equal(outPut?.Data?.IsActive, categoryDB.IsActive);
        }

       

        [Theory(DisplayName = nameof(ShoudReturErroWhenCreateCategoryWithInvalidName))]
        [Trait("Integration-Application", "Category Use Case")]
        [InlineData("nn")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("marckvaldo marckvlado marckvaldo markvaldo marckvaldo marckvaldo")]
        public async void ShoudReturErroWhenCreateCategoryWithInvalidName(string name)
        {
            var request = Faker();
            request.Name = name;

            var useCase = new ApplicationUseCase.CreateCategory(
                _notification,
                _categoryRepository,
                _unitOfWork);

            var action = async () => await useCase.Handle(request, CancellationToken.None);

            var exception = Assert.ThrowsAsync<EntityValidationException>(action);

            var categoryDB = await _categoryPersistence.GetAllCategory();

            Assert.True(categoryDB.Count() == 0) ;
        }


        public void Dispose()
        {
            CleanInMemoryDatabase();
        }
    }
}
