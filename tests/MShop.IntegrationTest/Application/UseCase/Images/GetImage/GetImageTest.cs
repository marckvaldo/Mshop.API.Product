﻿using MShop.Application.UseCases.Images.GetImage;
using MShop.Business.Interface.Service;
using MShop.Business.Service;
using MShop.Core.Message;
using MShop.IntegrationTests.Application.UseCase.Images.Commom;
using MShop.Repository.Context;
using MShop.Repository.Interface;
using MShop.Repository.Repository;
using ApplicationUseCase = MShop.Application.UseCases.Images.GetImage;

namespace MShop.IntegrationTests.Application.UseCase.Images.GetImage
{
    [Collection("GetImage Collection")]
    [CollectionDefinition("GetImage Collection", DisableParallelization = true)]
    public class GetImageTest : ImageTestFixture, IDisposable
    {
        private readonly IImageRepository _imageRepository;
        private readonly INotification _notification;
        private readonly RepositoryDbContext _repositoryContext;
        private readonly IStorageService _storageService;
        private readonly ImagePersistense _imagePersistense;

        public GetImageTest()
        {
            _repositoryContext = CreateDBContext();
            _imageRepository = new ImagesRepository(_repositoryContext);
            _notification = new Notifications();
            _storageService = new StorageService();
            _imagePersistense = new ImagePersistense(_repositoryContext);
        }

        [Fact(DisplayName = nameof(GetImage))]
        [Trait("Integration-Application", "Image Use Case")]
        public async Task GetImage()
        {

            var imageFake = Faker(Guid.NewGuid());
            await _imagePersistense.Create(imageFake);

            var guid = imageFake.Id;
            var useCase = new ApplicationUseCase.GetImage(_notification,_imageRepository,_storageService);
            var outPut = await useCase.Handle(new GetImageInPut(guid) ,CancellationToken.None);

            var result = outPut.Data;

            Assert.False(_notification.HasErrors());
            Assert.NotNull(result);
            Assert.Equal(result.ProductId, imageFake.ProductId);
            Assert.NotNull(result.Image);

        }

        public void Dispose()
        {
            CleanInMemoryDatabase();
        }
    }
}
