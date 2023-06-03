using MShop.Business.Interface.Repository;
using MShop.Business.Interface;
using MShop.IntegrationTests.Application.UseCase.Category.Common;
using MShop.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MShop.Business.Validation;
using MShop.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using ApplicationUseCase = MShop.Application.UseCases.Category.UpdateCategory;
using MShop.Application.Event;
using MShop.Repository.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

            var serviceColletion = new ServiceCollection();
            serviceColletion.AddLogging();
            var serviceProvider = serviceColletion.BuildServiceProvider();

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
            var outPut = await useCase.Handler(request, CancellationToken.None);

            var categoryDb = await _categoryPersistence.GetCategory(category.Id);

            Assert.NotNull(outPut);
            Assert.NotNull(categoryDb);
            Assert.Equal(outPut.Name, categoryDb.Name);
            Assert.NotEmpty(outPut.Name);
        }

        public void Dispose()
        {
            CleanInMemoryDatabase();
        }
    }
}
