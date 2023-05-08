using Microsoft.EntityFrameworkCore;
using Moq;
using MShop.Business.Exception;
using MShop.Business.Exceptions;
using MShop.Business.Interface;
using MShop.Business.Interface.Service;
using MShop.Business.Service;
using MShop.Business.Validation;
using MShop.IntegrationTests.Application.UseCase.Category;
using MShop.Repository.Context;
using MShop.Repository.Repository;
using System.Data.SqlTypes;
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
        }

        [Fact(DisplayName = nameof(CreateProduct))]
        [Trait("Integration-Application", "Product Use Case")]
        public async Task CreateProduct()
        {

           // var notification = new Notifications();
            var request = Faker();
            var categoryFake = FakeCategory();

            /*_DbContext.Categories.Add(categoryFake);
            await _DbContext.SaveChangesAsync();
            var categoryDb = _DbContext.Categories.FirstOrDefault();

            Assert.NotNull(categoryDb);           
            request.CategoryId = categoryDb.Id;*/

            await _categoryPersistence.Create(categoryFake);
            var categoryDb = await _categoryPersistence.GetCategory(categoryFake.Id);
            Assert.NotNull(categoryDb);
            request.CategoryId = categoryDb.Id;

            var productUseCase = new ApplicationUseCase.CreateProduct(_repository, _notification,_categoryRepository, _storageService, _imageRepository);
            var outPut = await productUseCase.Handler(request);

            //var newProduct = await CreateDBContext(true).Products.FindAsync(outPut.Id);
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

            //var notification = new Notifications();
            var request = Faker();

            request.CategoryId = Guid.NewGuid();

            var productUseCase = new ApplicationUseCase.CreateProduct(_repository, _notification, _categoryRepository, _storageService, _imageRepository);
            var outPut = async () => await productUseCase.Handler(request);

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
