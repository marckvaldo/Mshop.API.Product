using MShop.Business.Exceptions;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using MShop.Business.Interface.Service;
using MShop.Business.Service;
using MShop.Business.Validation;
using MShop.IntegrationTests.Application.UseCase.Category;
using MShop.IntegrationTests.Application.UseCase.Images.Commom;
using MShop.IntegrationTests.Application.UseCase.Images.CreateImage;
using MShop.IntegrationTests.Application.UseCase.Product;
using MShop.Repository.Context;
using MShop.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationUseCase = MShop.Application.UseCases.images.CreateImage;

namespace MShop.IntegrationTests.Application.UseCase.Images.CreateImages
{
    [Collection("Create Image Collection")]
    [CollectionDefinition("Create Image Collection", DisableParallelization = true)]
    public class CreateImageTest : CreateImageTestFixture, IDisposable
    {
        private readonly IImageRepository _imageRepository;
        private readonly IProductRepository _productRepository;
        private readonly INotification _notification;
        private readonly RepositoryDbContext _repositoryContext;
        private readonly IStorageService _storageService;
        private readonly ImagePersistense _imagePersistense;
        private readonly CategoryPersistence _categoryPersistence;
        private readonly ProductPersistence _productPersistence;

        public CreateImageTest()
        {
            _repositoryContext = CreateDBContext();
            _imageRepository = new ImagesRepository(_repositoryContext);
            _productRepository = new ProductRepository(_repositoryContext);
            _notification = new Notifications();
            _storageService = new StorageService();
            _imagePersistense = new ImagePersistense(_repositoryContext);
            _categoryPersistence = new CategoryPersistence(_repositoryContext); 
            _productPersistence = new ProductPersistence(_repositoryContext);
        }

        [Fact(DisplayName = nameof(CreateImage))]
        [Trait("Integration-Application", "Image Use Case")]
        public async void CreateImage()
        {
            var category = FakerCategory();
            await _categoryPersistence.Create(category);

            var product = FakerProduct(category);
            await _productPersistence.Create(product);  

            var request = FakerRequest(product.Id);
            var useCase = new ApplicationUseCase.CreateImage(_imageRepository, _storageService, _productRepository, _notification);
            var outPut = await useCase.Handler(request);

            Assert.NotNull(outPut);
            Assert.True(outPut.Images.Count == 3);
        }

        public void Dispose()
        {
            CleanInMemoryDatabase();
        }
    }
}
