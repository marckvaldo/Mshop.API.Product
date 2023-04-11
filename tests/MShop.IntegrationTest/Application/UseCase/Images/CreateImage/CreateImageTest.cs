using MShop.Business.Exceptions;
using MShop.Business.Interface;
using MShop.Business.Interface.Repository;
using MShop.Business.Interface.Service;
using MShop.Business.Service;
using MShop.Business.Validation;
using MShop.IntegrationTests.Application.UseCase.Images.Commom;
using MShop.IntegrationTests.Application.UseCase.Images.CreateImage;
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
        private readonly INotification _notification;
        private readonly RepositoryDbContext _repositoryContext;
        private readonly IStorageService _storageService;
        private readonly ImagePersistense _imagePersistense;

        public CreateImageTest()
        {
            _repositoryContext = CreateDBContext();
            _imageRepository = new ImagesRepository(_repositoryContext);
            _notification = new Notifications();
            _storageService = new StorageService();
            _imagePersistense = new ImagePersistense(_repositoryContext);
        }

        [Fact(DisplayName = nameof(CreateImage))]
        [Trait("Integration-Application", "Image Use Case")]
        public async void CreateImage()
        {
            var request = FakerRequest(Guid.NewGuid());
            var useCase = new ApplicationUseCase.CreateImage(_imageRepository, _storageService, _notification);
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
