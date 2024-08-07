﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MShop.Application.Event;
using MShop.Core.Exception;
using MShop.Core.Message;
using MShop.IntegrationTests.Application.UseCase.Product;
using MShop.Repository.Context;
using MShop.Repository.Interface;
using MShop.Repository.Repository;
using MShop.Repository.UnitOfWork;
using ApplicationUseCase = MShop.Application.UseCases.Category.DeleteCategory;

namespace MShop.IntegrationTests.Application.UseCase.Category.DeleteCategory
{

    [Collection("Delete Category Collection")]
    [CollectionDefinition("Delete Category Collection", DisableParallelization = true)]
    public class DeleteCategoryTest : DeleteCategoryTestFixture, IDisposable
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly INotification _notification;
        private readonly RepositoryDbContext _context;
        private readonly CategoryPersistence _categoryPersistence;
        private readonly ProductPersistence _productPersistence;
        private readonly UnitOfWork _unitOfWork;
        private readonly DomainEventPublisher _domainEventPublisher;

        public DeleteCategoryTest()
        {
            _context = CreateDBContext();
            _categoryRepository = new CategoryRepository(_context);
            _productRepository = new ProductRepository(_context);
            _notification = new Notifications();
            _categoryPersistence = new CategoryPersistence(_context);
            _productPersistence = new ProductPersistence(_context);

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

        [Fact(DisplayName = nameof(DeleteCategory))]
        [Trait("Integration-Application", "Category Use Case")]

        public async void DeleteCategory()
        {
            var categorys = FakerList(10);
            await _categoryPersistence.CreateList(categorys);

            var category = categorys.FirstOrDefault();
            Assert.NotNull(category);

            var useCase = new ApplicationUseCase.DeleteCategory(
                _categoryRepository,
                _productRepository,
                _notification,
                _unitOfWork);

            await useCase.Handle(new ApplicationUseCase.DeleteCategoryInPut(category.Id), CancellationToken.None);

            var categoryDB = await _categoryPersistence.GetCategory(category.Id);

            Assert.Null(categoryDB);
        }


        [Fact(DisplayName = nameof(ShoudRetunrErrorWhenDeleteCategoryThatThereAreProdutcs))]
        [Trait("Integration-Application", "Category Use Case")]

        public async void ShoudRetunrErrorWhenDeleteCategoryThatThereAreProdutcs()
        {
            var categorys = FakerList(10);
            await _categoryPersistence.CreateList(categorys);

            var category = categorys.FirstOrDefault();
            Assert.NotNull(category);

            var products = FakerProducts(category.Id,10);
            await _productPersistence.CreateList(products);
           

            var useCase = new ApplicationUseCase.DeleteCategory(
                _categoryRepository, 
                _productRepository, 
                _notification, 
                _unitOfWork);

            var action = async () => await useCase.Handle(new ApplicationUseCase.DeleteCategoryInPut(category.Id), CancellationToken.None);

            var exception = Assert.ThrowsAsync<ApplicationValidationException>(action);

            var categoryDB = await _categoryPersistence.GetCategory(category.Id);
            var productsDB = await _productPersistence.GetAllProduct();

            Assert.NotNull(categoryDB);
            Assert.True(productsDB.Count() == 10 );
        }

        public void Dispose()
        {
            CleanInMemoryDatabase();
        }
    }
}


