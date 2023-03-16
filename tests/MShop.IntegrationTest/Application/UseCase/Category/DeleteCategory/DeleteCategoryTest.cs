using MShop.Business.Interface.Repository;
using MShop.Business.Interface;
using MShop.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MShop.Business.Validation;
using MShop.Repository.Repository;
using ApplicationUseCase = MShop.Application.UseCases.Category.DeleteCategory;
using MShop.IntegrationTests.Application.UseCase.Product;
using MShop.Business.Entity;
using MShop.Business.Exceptions;

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


        public DeleteCategoryTest()
        {
            _context = CreateDBContext();
            _categoryRepository = new CategoryRepository(_context);
            _productRepository = new ProductRepository(_context);
            _notification = new Notifications();
            _categoryPersistence = new CategoryPersistence(_context);
            _productPersistence = new ProductPersistence(_context);
        }

        [Fact(DisplayName = nameof(DeleteCategory))]
        [Trait("Integration-Application", "Delete Use Case")]

        public async void DeleteCategory()
        {
            var categorys = FakerList(10);
            await _categoryPersistence.CreateList(categorys);

            var category = categorys.FirstOrDefault();
            Assert.NotNull(category);

            var useCase = new ApplicationUseCase.DeleteCategory(_categoryRepository,_productRepository,_notification);
            await useCase.Handler(category.Id);

            var categoryDB = await _categoryPersistence.GetCategory(category.Id);

            Assert.Null(categoryDB);
        }


        [Fact(DisplayName = nameof(ShoutRetunrErrorWhenCategoryWhereThereAreProdutcs))]
        [Trait("Integration-Application", "Delete Use Case")]

        public async void ShoutRetunrErrorWhenCategoryWhereThereAreProdutcs()
        {
            var categorys = FakerList(10);
            await _categoryPersistence.CreateList(categorys);

            var category = categorys.FirstOrDefault();
            Assert.NotNull(category);

            var products = FakerProducts(category.Id,10);
            _productPersistence.CreateList(products);
           

            var useCase = new ApplicationUseCase.DeleteCategory(_categoryRepository, _productRepository, _notification);
            var action = async () => await useCase.Handler(category.Id);

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


