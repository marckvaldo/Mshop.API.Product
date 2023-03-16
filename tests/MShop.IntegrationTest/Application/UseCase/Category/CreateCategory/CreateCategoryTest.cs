using ApplicationUseCase = MShop.Application.UseCases.Category.CreateCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MShop.Business.Interface.Repository;
using MShop.Business.Interface;
using MShop.Repository.Repository;
using MShop.Business.Validation;
using MShop.Repository.Context;
using Microsoft.EntityFrameworkCore;
using MShop.Business.Exceptions;

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

        public CreateCategoryTest()
        {
            _context = CreateDBContext();
            _categoryRepository = new CategoryRepository(_context);
            _notification = new Notifications();
            _categoryPersistence = new CategoryPersistence(_context);   
        }

        [Fact(DisplayName = nameof(CreateCategory))]
        [Trait("Integration-Application", "Category Use Case")]
        public async void CreateCategory()
        {
            var request = Faker();
            var useCase = new ApplicationUseCase.CreateCategory(_notification,_categoryRepository);
            var outPut = await useCase.Handler(request);

            var categoryDB = await _categoryPersistence.GetCategory(outPut.Id);

            Assert.Equal(outPut.Name, categoryDB.Name);
            Assert.Equal(outPut.IsActive, categoryDB.IsActive);
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

            var useCase = new ApplicationUseCase.CreateCategory(_notification, _categoryRepository);
            var action = async () => await useCase.Handler(request);

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
