using MShop.Business.Interface.Repository;
using MShop.Business.Interface.Service;
using MShop.Business.Interface;
using MShop.Business.Service;
using MShop.Business.Validation;
using MShop.IntegrationTests.Application.UseCase.Images.Commom;
using MShop.Repository.Context;
using MShop.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationUseCase = MShop.Application.UseCases.images.DeleteImage;

namespace MShop.IntegrationTests.Application.UseCase.Images.DeleteImage
{
    [Collection("Delete Image Collection")]
    [CollectionDefinition("Delete Image Collection", DisableParallelization = true)]
    public class DeleteImageTest : ImageTestFixture, IDisposable
    {
        private readonly IImageRepository _imageRepository;
        private readonly INotification _notification;
        private readonly RepositoryDbContext _repositoryContext;
        private readonly IStorageService _storageService;
        private readonly ImagePersistense _imagePersistense;

        public DeleteImageTest()
        {
            _repositoryContext = CreateDBContext();
            _imageRepository = new ImagesRepository(_repositoryContext);
            _notification = new Notifications();
            _storageService = new StorageService();
            _imagePersistense = new ImagePersistense(_repositoryContext);
        }

        [Fact(DisplayName = nameof(DeleteImage))]
        [Trait("Integration-Application", "Image Use Case")]

        public async Task DeleteImage()
        {
            var image = Faker(Guid.NewGuid());

            await _imagePersistense.Create(image);

            var useCase = new ApplicationUseCase.DeleteImage(_imageRepository, _storageService, _notification);
            await useCase.Handler(image.Id);

            var imageDbDelete = await _imagePersistense.GetImage(image.Id);

            Assert.Null(imageDbDelete);

        }

        public void Dispose()
        {
            CleanInMemoryDatabase();
        }
    }
}
