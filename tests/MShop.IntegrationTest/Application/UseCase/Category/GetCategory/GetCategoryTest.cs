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
using MShop.IntegrationTests.Application.UseCase.Category.Common;
using Microsoft.EntityFrameworkCore;
using ApplicationUseCase = MShop.Application.UseCases.Category.GetCatetory;
using MShop.Business.Exception;

namespace MShop.IntegrationTests.Application.UseCase.Category.GetCategory
{
    [Collection("Get Category Collection")]
    [CollectionDefinition("Get Category Collection", DisableParallelization = true)]
    public class GetCategoryTest : CategoryTestFixture, IDisposable
    {
        private readonly ICategoryRepository _categoryRepository;
        //private readonly IProductRepository _productRepository;
        private readonly INotification _notification;
        private readonly RepositoryDbContext _context;
        private readonly CategoryPersistence _categoryPersistence;

        public GetCategoryTest()
        {
            _context = CreateDBContext();
            _categoryRepository = new CategoryRepository(_context);
           // _productRepository = new ProductRepository(_context);
            _notification = new Notifications();
            _categoryPersistence = new CategoryPersistence(_context);
        }

        [Fact(DisplayName = nameof(GetCategory))]
        [Trait("Integration-Application", "Category Use Case")]
        public async Task GetCategory()
        {
            var categoryFake = Faker();
            _categoryPersistence.Create(categoryFake);

            var guid = categoryFake.Id;
            var useCase = new ApplicationUseCase.GetCategory(_notification, _categoryRepository);
            var outPut = await useCase.Handler(guid);


            Assert.False(_notification.HasErrors());
            Assert.NotNull(outPut);
            Assert.Equal(outPut.Name, categoryFake.Name);
            Assert.Equal(outPut.IsActive, categoryFake.IsActive);

        }


        [Fact(DisplayName = nameof(SholdReturnErrorWhenCantGetProduct))]
        [Trait("Integration-Application", "Category Use Case")]
        public async Task SholdReturnErrorWhenCantGetProduct()
        {
            var categoryFake = Faker();
            _categoryPersistence.Create(categoryFake);


            var useCase = new ApplicationUseCase.GetCategory(_notification,_categoryRepository);
            var outPut = async () => await useCase.Handler(Guid.NewGuid());

            var exception = await Assert.ThrowsAsync<NotFoundException>(outPut);
            Assert.Equal("your search returned null", exception.Message);
            Assert.False(_notification.HasErrors());

        }

        public void Dispose()
        {
            CleanInMemoryDatabase();
        }

    }
}
