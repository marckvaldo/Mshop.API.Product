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
using ApplicationUseCase = MShop.Application.UseCases.Images.GetImage;
using MShop.Application.UseCases.Images.GetImage;

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


            Assert.False(_notification.HasErrors());
            Assert.NotNull(outPut);
            Assert.Equal(outPut.ProductId, imageFake.ProductId);
            Assert.NotNull(outPut.Image);

        }

        public void Dispose()
        {
            CleanInMemoryDatabase();
        }
    }
}
