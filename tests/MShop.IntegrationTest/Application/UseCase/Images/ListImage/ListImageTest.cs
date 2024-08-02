using MShop.Application.UseCases.Images.ListImage;
using MShop.Business.Interface.Service;
using MShop.Business.Service;
using MShop.Core.Message;
using MShop.IntegrationTests.Application.UseCase.Images.Commom;
using MShop.Repository.Context;
using MShop.Repository.Interface;
using MShop.Repository.Repository;
using ApplicationUseCase = MShop.Application.UseCases.Images.ListImage;

namespace MShop.IntegrationTests.Application.UseCase.Images.ListImage
{
    [Collection("List Image Collection")]
    [CollectionDefinition("List Image Collection", DisableParallelization = true)]
    public class ListImageTest : ImageTestFixture, IDisposable
    {
        private readonly IImageRepository _imageRepository;
        private readonly INotification _notification;
        private readonly RepositoryDbContext _repositoryContext;
        private readonly IStorageService _storageService;
        private readonly ImagePersistense _imagePersistense;

        public ListImageTest()
        {
            _repositoryContext = CreateDBContext();
            _imageRepository = new ImagesRepository(_repositoryContext);
            _notification = new Notifications();
            _storageService = new StorageService();
            _imagePersistense = new ImagePersistense(_repositoryContext);
        }

        [Fact(DisplayName = nameof(ListImage))]
        [Trait("Integration-Application", "Image Use Case")]
        public async Task ListImage()
        {
            var quantity = 20;
            var productId = Guid.NewGuid();
            var imagesFake = FakerList(productId, quantity);
            await _imagePersistense.CreateList(imagesFake);
         
            var useCase = new ApplicationUseCase.ListImage(_notification, _imageRepository);
            var outPut = await useCase.Handle(new ListImageInPut(productId), CancellationToken.None);

            var result = outPut.Data;

            Assert.NotNull(result);
            Assert.Equal(imagesFake.Count, quantity);
            Assert.Equal(result.Images.Count, quantity);
            foreach (var item in result.Images) 
            {
                var image = result.Images.Where(i => i.Image == item.Image);
                Assert.NotNull(image);
            }
        }

        public void Dispose()
        {
            CleanInMemoryDatabase();
        }
    }
}
